param($installPath, $toolsPath, $package, $project)

	# Aborts execution and effects a rollback if the condition is false.
	function Assert([bool]$condition, [string]$errorMessage)
	{		
		if (-not $condition)
		{
			throw $errorMessage
		}
	}
	
	# Gets number of elements in a list
	function GetCount($list)
	{	    
	    $count = 0
	    if ($list -ne $null)
	    {
	        foreach ($element in $list)
	        {
	            $count++
	        }
	    }
	    
	    return $count
	}
	
	# Ensures the list has a single element, and returns that element
	function GetSingle($list, [string]$errorMessageZero, [string]$errorMessagePlural)
	{
		$count = GetCount($list)
		
		Assert ($count -ne 0) $errorMessageZero
		Assert ($count -eq 1) $errorMessagePlural
		
		foreach ($element in $list)
		{
			return $element
		}
	}
	
    # Ensures the list has at least one element
    function AssertListNotEmpty($list, [string]$errorMessageEmpty)
    {
		$count = GetCount($list)
		Assert ($count -ne 0) $errorMessageEmpty
    }

	# Checks that a file starts with a 3-letter extension of the form (.xxx)
	function CheckExtension([string]$file, [string]$extPrefix)
	{
		return [System.IO.Path]::GetExtension($file).StartsWith($extPrefix, [StringComparison]::OrdinalIgnoreCase)
	}

    function MapTraceLevelToClientLevel([string]$level)
    {
        switch ($level)
        {
            { $_ -eq "Off" } { return '0' }
            { $_ -eq "Error" } { return '1' }
            { $_ -eq "Warning" } { return '2' }
            { $_ -eq "Info" } { return '3' }
            { $_ -eq "Verbose" } { return '4' }
            default { throw "Incorrect 'traceLevel'. Possible values are 'Off', 'Error', 'Warning', 'Info' and 'Verbose'." }
        }
    }


    # The content files for caching to have copy local as true
    function SetCachingBuildItemsAsCopyLocal()
    {
        foreach ($item in $project.ProjectItems | where { $_.Name -eq 'Microsoft.WindowsAzure.Caching' } | %{ $_.ProjectItems })
        {
            $item.Properties.Item("BuildAction").Value = 0
            $item.Properties.Item("CopyToOutputDirectory").Value = 1
        }
    }

    # Save app.config and web.config files
    function SaveProjectConfigs($configFilesXmlTable)
    {
        foreach ($configXml in $configFilesXmlTable.Keys)
        {
            $configXml.Save($configFilesXmlTable.Get_Item($configXml))
        }
    }
    
	# Return a fileWriter with prettified xml settings.
	function PrettyXmlWriter($filepath)
	{
		$fileWriter = New-Object System.Xml.XmlTextWriter($filepath, $null)
		$fileWriter.Formatting = [System.Xml.Formatting]::Indented
		$fileWriter.Indentation = 4
		$fileWriter.IndentChar = ' '
		$fileWriter.Flush() 
		return $fileWriter
	}

# MAIN

$projectName = $project.Name
#Assert ($project.Saved) "Save the project $projectName before continuing."

$solution = $project.DTE.Solution
$ccProjects = $solution.Projects | where { $_.Kind -eq '{cc5fd16d-436d-48ad-a40c-5a424c6e3e79}' -and (CheckExtension $_.FileName '.ccp') }
$filteredProjects = @()
foreach ($curProject in $ccProjects)
{
	$rolesSections = $curProject.ProjectItems | where { $_.GetType().FullName -eq 'Microsoft.VisualStudio.Project.Automation.OAReferenceFolderItem' }
	if ($(GetCount $rolesSections) -eq 1)
	{
		$roles = $rolesSections | %{ $_.ProjectItems } | where { $_.Object.SourceProject.UniqueName -eq $project.UniqueName }
		if ($(GetCount $roles) -eq 1)
		{
			$filteredProjects += $curProject
		}
	}
}

$filteredProjectsCount = GetCount $filteredProjects
Assert ($filteredProjectsCount -le 1) "More than one Windows Azure project including a Role for $projectName were found in this solution."

$ccProject = $filteredProjects[0]

$traceLevelFromConfig = "Error"
$autoDiscoverNodeName = "autoDiscover"
$dataCacheClientNodeName = "dataCacheClient"
$dataCacheClientsNodeName = "dataCacheClients"
$configSectionsNodeName = "configSections"
# Segment 1 of 4: Rectify app/web.config

$configFiles = $project.ProjectItems | where { ($_.Name -eq "app.config") -or ($_.Name -eq "web.config") }
$configFilesXmlTable = @{}

# Only one config file is expected in reality
foreach ($configFile in $configFiles)
{
    $configFileName = $configFile.Name
    $configFilePath = $configFile.Properties.Item("FullPath").Value
    Assert (Test-Path $configFilePath) "The file $configFileName in project $projectName was not found. Check if the file exists at $configFilePath"

    $configXml = New-Object XML
    $configXml.PreserveWhitespace = $true
    $configXml.LoadXml([System.IO.File]::ReadAllText($configFilePath))
    $configFilesXmlTable.Add($configXml, $configFilePath)

    $nsMgr = New-Object System.Xml.XmlNamespaceManager($configXml.NameTable)

    # Config update 1 of 5: Remove redundant autoDiscovery elements
    $DataCacheClientName = "default"
    $dataCacheClientsNode = $configXml.DocumentElement.SelectSingleNode($dataCacheClientsNodeName)
    $defaultCacheClientNode = $dataCacheClientsNode.SelectSingleNode("dataCacheClient[@name='$DataCacheClientName']", $nsMgr)
    $autoDiscoverNodes = $defaultCacheClientNode.SelectNodes($autoDiscoverNodeName)
    if ($autoDiscoverNodes.Count -gt 1)
    {
        $redundantNodes = $defaultCacheClientNode.SelectNodes('autoDiscover[@identifier="[Cache role name or Service Endpoint]"]')
        foreach ($redundantNode in $redundantNodes)
        {
            $defaultCacheClientNode.RemoveChild($redundantNode)
        }
    }

	
    # Config update 2 of 5: Remove redundant crashDump elements
    $cacheDiagnosticsNode = $configXml.DocumentElement.SelectSingleNode("cacheDiagnostics")
    $crashDumpNodes = $cacheDiagnosticsNode.SelectNodes("crashDump", $nsMgr)
    if ($crashDumpNodes.Count -gt 1)
    {
        $redundantNodes = $cacheDiagnosticsNode.SelectNodes("crashDump[@dumpLevel='Off' and @dumpStorageQuotaInMB='100']", $nsMgr)
        foreach ($redundantNode in $redundantNodes)
        {
            $cacheDiagnosticsNode.RemoveChild($redundantNode)	
        }
    }
	
	#Config update 3 of 5: Remove cacheDiagnostic section for non-cloud project
	if($filteredProjectsCount -lt 1)
	{
		$configXml.DocumentElement.RemoveChild($cacheDiagnosticsNode)
		
	}

	#Config update 4 of 5: Remove VAS configuration node 'hosts' and 'securityProperties' (for VAS upgrade scenario)
	$dataCacheClientNodes = $dataCacheClientsNode.SelectNodes($dataCacheClientNodeName)
	foreach($dataCacheClientNode in $dataCacheClientNodes)
	{
		$hostsNode = $dataCacheClientNode.SelectSingleNode("hosts")
		if ($hostsNode -ne $null)
		{
			$dataCacheClientNode.RemoveChild($hostsNode);
		}
		
		$securityPropertiesNode = $dataCacheClientNode.SelectSingleNode("securityProperties")
		if ($securityPropertiesNode -ne $null)
		{
			$dataCacheClientNode.RemoveChild($securityPropertiesNode);
		}
		
		# Clean empty <dataCacheClient /> tag (left from VAS) 
		if (!($dataCacheClientNode.InnerXml.TrimEnd()))
		{
			$dataCacheClientsNode.RemoveChild($dataCacheClientNode);
		}
	}
	
	# Config update 5 of 5: Get traceLevel to be later mapped to clientDiagnosticsLevel
    $tracingNode = $dataCacheClientsNode.SelectSingleNode('tracing', $nsMgr)
    if ($tracingNode -ne $null)
    {
        $traceLevelAttrValue = $tracingNode.Attributes.ItemOf('traceLevel').Value
        if ($traceLevelAttrValue -ne $null)
        {
            $traceLevelFromConfig = $traceLevelAttrValue
        }

        $dataCacheClientsNode.RemoveChild($tracingNode)
    }
}


if ($filteredProjectsCount -lt 1)
{
    # Since rest of the segments deal with ccproj(not applicable to IaaS case), skip to last segment
    # Segment 4 of 4: Commit changes
    SetCachingBuildItemsAsCopyLocal
    SaveProjectConfigs $configFilesXmlTable
    
    return
}


# The .ccproj file is used by Azure to generate the CSPKG.
$ccProjectName = $ccProject.Name

# The ccproj uses the References section to encapsulate what are essentially Roles.
#
$rolesSections = $ccProject.ProjectItems | where { $_.GetType().FullName -eq 'Microsoft.VisualStudio.Project.Automation.OAReferenceFolderItem' }
$rolesSection = GetSingle $rolesSections "The Windows Azure Project $ccProjectName does not have a Roles section." "The Windows Azure Project $ccProjectName has duplicate Roles section defined."
$roleNames = $rolesSection.ProjectItems | where { $_.Object.SourceProject.UniqueName -eq $project.UniqueName } | %{ $_.Name }
$roleName = GetSingle $roleNames "The Windows Azure Project $ccProjectName does not include a Role for $projectName" "The Windows Azure Project $ccProjectName has duplicate Role entries referring to $projectName."


# Segment 2 of 4: Update CSDEF

$csdefFiles = $ccProject.ProjectItems | where { $_.Properties -ne $null -and $_.Properties.Item('BuildAction') -ne $null -and $_.Properties.Item('BuildAction').Value -eq 'ServiceDefinition' }
$csdefFile = GetSingle $csdefFiles "The Windows Azure Project $ccProjectName does not have a ServiceDefinition (CSDEF) file." "The Windows Azure Project $ccProjectName has more than one ServiceDefinition (CSDEF) files."

$csdefFileName = $csdefFile.Name
Assert $csdefFile.Saved "Save the file $csdefFileName in project $ccProjectName before continuing."

$csdefFilePath = $csdefFile.Object.Url
Assert (Test-Path $csdefFilePath) "The file $csdefFileName in project $ccProjectName was not found. Check if the file exists at $csdefFilePath"

# This roundabout means to loading the XML file is to ensure whitespace is preserved.
$csdefXml = New-Object XML
$csdefXml.LoadXml([System.IO.File]::ReadAllText($csdefFilePath))
$nsMgr = New-Object System.Xml.XmlNamespaceManager($csdefXml.NameTable)
$csdefNamespace = 'http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition'
$nsMgr.AddNamespace('sd', $csdefNamespace)

$requiredWebRole = $csdefXml.DocumentElement.SelectSingleNode("/sd:ServiceDefinition/sd:WebRole[@name='$roleName']", $nsMgr)
$requiredWorkerRole = $csdefXml.DocumentElement.SelectSingleNode("/sd:ServiceDefinition/sd:WorkerRole[@name='$roleName']", $nsMgr)

$csdefRoleNode = $requiredWebRole
if ($requiredWorkerRole -ne $null)
{
    Assert ($csdefRoleNode -eq $null) "The ServiceDefinition file $csdefFileName in project $ccProjectName is corrupt."
    $csdefRoleNode = $requiredWorkerRole
}

Assert ($csdefRoleNode -ne $null) "The ServiceDefinition file $csdefFileName for project $ccProjectName does not include a WebRole/WorkerRole section named $roleName."

# csdef update 1 of 3: Add startup task
$startupNode = $csdefRoleNode.SelectSingleNode("sd:Startup", $nsMgr)
if ($startupNode -eq $null)
{
    $startupNode = $csdefXml.CreateElement("Startup", $csdefNamespace)
    $csdefRoleNode.AppendChild($startupNode) | Out-Null
}

if ($startupNode.Attributes.ItemOf('priority') -eq $null)
{
    $startupPriorityAttribute = $csdefXml.CreateAttribute('priority')
    $startupPriorityAttribute.Value = '-2'
    $startupNode.Attributes.Append($startupPriorityAttribute) | Out-Null
}

$startupTaskNode = $csdefXml.CreateElement('Task', $csdefNamespace)
$startupNode.AppendChild($startupTaskNode) | Out-Null

$startupTaskCommandLine = $csdefXml.CreateAttribute('commandLine')
$startupTaskCommandLine.Value = 'Microsoft.WindowsAzure.Caching\ClientPerfCountersInstaller.exe install'
$startupTaskNode.Attributes.Append($startupTaskCommandLine) | Out-Null

$startupTaskExecutionContext = $csdefXml.CreateAttribute('executionContext')
$startupTaskExecutionContext.Value = 'elevated'
$startupTaskNode.Attributes.Append($startupTaskExecutionContext) | Out-Null

$startupTaskTaskType = $csdefXml.CreateAttribute('taskType')
$startupTaskTaskType.Value = 'simple'
$startupTaskNode.Attributes.Append($startupTaskTaskType) | Out-Null

# csdef update 2 of 3: Add config setting ClientDiagnosticLevel
$csdefConfigSettingsNode = $csdefRoleNode.SelectSingleNode("sd:ConfigurationSettings", $nsMgr)
if ($csdefConfigSettingsNode -eq $null)
{
    $csdefConfigSettingsNode = $csdefXml.CreateElement("ConfigurationSettings", $csdefNamespace)
    $csdefRoleNode.AppendChild($csdefConfigSettingsNode) | Out-Null
}

$csdefSettingNode = $csdefConfigSettingsNode.SelectSingleNode('sd:Setting[@name="Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel"]', $nsMgr)
if ($csdefSettingNode -eq $null)
{
    $csdefSettingNode = $csdefXml.CreateElement("Setting", $csdefNamespace)
    $settingNameAttribute = $csdefXml.CreateAttribute('name')
    $settingNameAttribute.Value = "Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel"
    $csdefSettingNode.Attributes.Append($settingNameAttribute) | Out-Null
    $csdefConfigSettingsNode.AppendChild($csdefSettingNode) | Out-Null
}


# csdef update 3 of 3: Import Diagnostics module
$csdefImportsNode = $csdefRoleNode.SelectSingleNode("sd:Imports", $nsMgr)
if ($csdefImportsNode -eq $null)
{
    $csdefImportsNode = $csdefXml.CreateElement("Imports", $csdefNamespace)
    $csdefRoleNode.AppendChild($csdefImportsNode) | Out-Null
}

$csdefImportModuleDiagnosticsNode = $csdefImportsNode.SelectSingleNode('sd:Import[@moduleName="Diagnostics"]', $nsMgr)
if ($csdefImportModuleDiagnosticsNode -eq $null)
{
    $csdefImportModuleDiagnosticsNode = $csdefXml.CreateElement("Import", $csdefNamespace)
    $moduleNameAttribute = $csdefXml.CreateAttribute('moduleName')
    $moduleNameAttribute.Value = "Diagnostics"
    $csdefImportModuleDiagnosticsNode.Attributes.Append($moduleNameAttribute) | Out-Null
    $csdefImportsNode.AppendChild($csdefImportModuleDiagnosticsNode) | Out-Null
}



# Segment 3 of 4: Update CSCFG(s)

$cscfgFiles = $ccProject.ProjectItems | where { $_.Properties -ne $null -and $_.Properties.Item('BuildAction') -ne $null -and $_.Properties.Item('BuildAction').Value -eq 'ServiceConfiguration' }
AssertListNotEmpty $cscfgFiles "The Windows Azure Project $ccProjectName does not have a ServiceConfiguration (CSCFG) file."

$cscfgNamespace = 'http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration'
$cscfgXmlTable = @{}

foreach ($cscfgFile in $cscfgFiles)
{
    $cscfgFileName = $cscfgFile.Name
    Assert $cscfgFile.Saved "Save the file $cscfgFileName in project $ccProjectName before continuing."

    $cscfgFilePath = $cscfgFile.Object.Url
    Assert (Test-Path $cscfgFilePath) "The file $cscfgFileName in project $ccProjectName was not found. Check if the file exists at $cscfgFilePath"

    # Ensure whitespace is preserved.
    $cscfgXml = New-Object XML
    $cscfgXml.LoadXml([System.IO.File]::ReadAllText($cscfgFilePath))
    $cscfgXmlTable.Add($cscfgXml, $cscfgFilePath)
    
    $nsMgr = New-Object System.Xml.XmlNamespaceManager($cscfgXml.NameTable)
    $nsMgr.AddNamespace('sd', $cscfgNamespace)

    $cscfgRoleNode = $cscfgXml.DocumentElement.SelectSingleNode("/sd:ServiceConfiguration/sd:Role[@name='$roleName']", $nsMgr)
    Assert ($cscfgRoleNode -ne $null) "The ServiceConfiguration file $cscfgFileName for project $ccProjectName does not include a Role section named $roleName."

    # cscfg update 1 of 1: Add default setting value for client diag level as '1'
    $cscfgConfigSettingsNode = $cscfgRoleNode.SelectSingleNode("sd:ConfigurationSettings", $nsMgr)
    if ($cscfgConfigSettingsNode -eq $null)
    {
        $cscfgConfigSettingsNode = $cscfgXml.CreateElement("ConfigurationSettings", $cscfgNamespace)
        $cscfgRoleNode.AppendChild($cscfgConfigSettingsNode) | Out-Null
    }

    $cscfgSettingNode = $cscfgConfigSettingsNode.SelectSingleNode('sd:Setting[@name="Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel"]', $nsMgr)
    if ($cscfgSettingNode -eq $null)
    {
        $cscfgSettingNode = $cscfgXml.CreateElement("Setting", $cscfgNamespace)
        
        $settingNameAttribute = $cscfgXml.CreateAttribute('name')
        $settingNameAttribute.Value = "Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel"
        $settingValueAttribute = $cscfgXml.CreateAttribute('value')
        $settingValueAttribute.Value = MapTraceLevelToClientLevel $traceLevelFromConfig
        
        $cscfgSettingNode.Attributes.Append($settingNameAttribute) | Out-Null
        $cscfgSettingNode.Attributes.Append($settingValueAttribute) | Out-Null
	    
        $cscfgConfigSettingsNode.AppendChild($cscfgSettingNode) | Out-Null
    }
}    



# Segment 4 of 4: Commit changes
# Any exception beyond this point requires the code below to rollback existing changes.

SetCachingBuildItemsAsCopyLocal
SaveProjectConfigs $configFilesXmlTable

# Save csdef and cscfg files
$csdefWriter = PrettyXmlWriter($csdefFilePath)
$csdefXml.Save($csdefWriter)
$csdefWriter.Close()

foreach ($cscfgXml in $cscfgXmlTable.Keys)
{
	$cscfgWriter = PrettyXmlWriter($cscfgXmlTable.Get_Item($cscfgXml))
	$cscfgXml.Save($cscfgWriter)
	$cscfgWriter.Close()
}

