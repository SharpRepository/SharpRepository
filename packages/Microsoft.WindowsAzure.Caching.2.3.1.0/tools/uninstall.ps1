param($installPath, $toolsPath, $package, $project)

	# Errors out and exits the rollback process
	function Assert([bool]$condition, [string]$errorMessage)
	{		
		if (-not $condition)
		{
			Write-Host $errorMessage
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
		
	# Checks that a file starts with a 3-letter extension of the form (.xxx)
	function CheckExtension([string]$file, [string]$extPrefix)
	{
		return [System.IO.Path]::GetExtension($file).StartsWith($extPrefix, [StringComparison]::OrdinalIgnoreCase)
	}
    
    # Ensures the list has at least one element
    function AssertListNotEmpty($list, [string]$errorMessageEmpty)
    {
		$count = GetCount($list)
		Assert ($count -ne 0) $errorMessageEmpty
    }    



# MAIN

$projectName = $project.Name

$solution = $project.DTE.Solution
$ccProjects = $solution.Projects | where { $_.Kind -eq '{cc5fd16d-436d-48ad-a40c-5a424c6e3e79}' -and (CheckExtension $_.FileName '.ccp') }

if ($(GetCount $ccProjects) -lt 1)
{
    return
}
else
{
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
    
    $ccProject = GetSingle $filteredProjects 'No Windows Azure project including a Role for $projectName was found in this solution.' 'More than one Windows Azure project including a Role for $projectName were found in this solution.'
}

$ccProjectName = $ccProject.Name

$rolesSections = $ccProject.ProjectItems | where { $_.GetType().FullName -eq 'Microsoft.VisualStudio.Project.Automation.OAReferenceFolderItem' }
$rolesSection = GetSingle $rolesSections "The Windows Azure Project $ccProjectName does not have a Roles section." "The Windows Azure Project $ccProjectName has duplicate Roles section defined."
$roleNames = $rolesSection.ProjectItems | where { $_.Object.SourceProject.UniqueName -eq $project.UniqueName } | %{ $_.Name }
$roleName = GetSingle $roleNames "The Windows Azure Project $ccProjectName does not include a Role for $projectName" "The Windows Azure Project $ccProjectName has duplicate Role entries referring to $projectName."

# Change 1 of 2: Update CSDEF
$csdefFiles = $ccProject.ProjectItems | where { $_.Properties -ne $null -and $_.Properties.Item('BuildAction') -ne $null -and $_.Properties.Item('BuildAction').Value -eq 'ServiceDefinition' }
$csdefFile = GetSingle $csdefFiles "The Windows Azure Project $ccProjectName does not have a ServiceDefinition (CSDEF) file." "The Windows Azure Project $ccProjectName has more than one ServiceDefinition (CSDEF) files."

$csdefFileName = $csdefFile.Name
Assert $csdefFile.Saved "Save the file $csdefFileName in project $ccProjectName before continuing."

$csdefFilePath = $csdefFile.Object.Url
Assert (Test-Path $csdefFilePath) "The file $csdefFileName in project $ccProjectName was not found. Check if the file exists at $csdefFilePath"

$csdefXml = New-Object XML
$csdefXml.PreserveWhitespace = $true
$csdefXml.LoadXml([System.IO.File]::ReadAllText($csdefFilePath))
$nsMgr = New-Object System.Xml.XmlNamespaceManager($csdefXml.NameTable)
$csdefNamespace = 'http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition'
$nsMgr.AddNamespace('sd', $csdefNamespace)

$requiredWebRole = $csdefXml.DocumentElement.SelectSingleNode("/sd:ServiceDefinition/sd:WebRole[@name='$roleName']", $nsmgr)
$requiredWorkerRole = $csdefXml.DocumentElement.SelectSingleNode("/sd:ServiceDefinition/sd:WorkerRole[@name='$roleName']", $nsmgr)

$csdefRoleNode = $requiredWebRole
if ($requiredWorkerRole -ne $null)
{
    Assert ($csdefRoleNode -eq $null) "The ServiceDefinition file $csdefFileName in project $ccProjectName is corrupt."
    $csdefRoleNode = $requiredWorkerRole
}

Assert ($csdefRoleNode -ne $null) "The ServiceDefinition file $csdefFileName for project $ccProjectName does not include a WebRole/WorkerRole section named $roleName."

# csdef update 1 of 2: Remove startup task
$startupNode = $csdefRoleNode.SelectSingleNode("sd:Startup", $nsMgr)
if ($startupNode -ne $null)
{
    $startupTaskNodes = $startupNode.SelectNodes('sd:Task[@commandLine="Microsoft.WindowsAzure.Caching\ClientPerfCountersInstaller.exe install"]', $nsmgr)
    foreach ($startupTaskNode in $startupTaskNodes)
    {
	    if ($startupTaskNode -ne $null)
	    {
		    $startupNode.RemoveChild($startupTaskNode) | Out-Null
	    }
    }

    # select only element nodes
    $startupTaskNodes = $startupNode.SelectNodes("*", $nsmgr)
    if ($startupTaskNodes.Count -eq 0)
    {
	    $csdefRoleNode.RemoveChild($startupNode) | Out-Null
    }
}

# csdef update 2 of 2: Remove config setting ClientDiagnosticLevel
$csdefConfigSettingsNode = $csdefRoleNode.SelectSingleNode("sd:ConfigurationSettings", $nsMgr)
if ($csdefConfigSettingsNode -ne $null)
{
    $settingNodes = $csdefConfigSettingsNode.SelectNodes('sd:Setting[@name="Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel"]', $nsmgr)
    foreach ($settingNode in $settingNodes)
    {
	    if ($settingNode -ne $null)
	    {
		    $csdefConfigSettingsNode.RemoveChild($settingNode) | Out-Null
	    }
    }

    # select only element nodes
    $settingNodes = $csdefConfigSettingsNode.SelectNodes("*", $nsmgr)
    if ($settingNodes.Count -eq 0)
    {
	    $csdefRoleNode.RemoveChild($csdefConfigSettingsNode) | Out-null
    }
}

$csdefXml.Save($csdefFilePath)



# Change 2 of 2: Update CSCFG(s)
$cscfgFiles = $ccProject.ProjectItems | where { $_.Properties -ne $null -and $_.Properties.Item('BuildAction') -ne $null -and $_.Properties.Item('BuildAction').Value -eq 'ServiceConfiguration' }
AssertListNotEmpty $cscfgFiles "The Windows Azure Project $ccProjectName does not have a ServiceConfiguration (CSCFG) file."

$cscfgNamespace = 'http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration'

foreach ($cscfgFile in $cscfgFiles)
{
    $cscfgFileName = $cscfgFile.Name
    Assert $cscfgFile.Saved "Save the file $cscfgFileName in project $ccProjectName before continuing."

    $cscfgFilePath = $cscfgFile.Object.Url
    Assert (Test-Path $cscfgFilePath) "The file $cscfgFileName in project $ccProjectName was not found. Check if the file exists at $cscfgFilePath"

    $cscfgXml = New-Object XML
    $cscfgXml.PreserveWhitespace = $true
    $cscfgXml.LoadXml([System.IO.File]::ReadAllText($cscfgFilePath))

    $nsMgr = New-Object System.Xml.XmlNamespaceManager($cscfgXml.NameTable)
    $nsMgr.AddNamespace('sd', $cscfgNamespace)

    $cscfgRoleNode = $cscfgXml.DocumentElement.SelectSingleNode("/sd:ServiceConfiguration/sd:Role[@name='$roleName']", $nsmgr)
    Assert ($cscfgRoleNode -ne $null) "The ServiceConfiguration file $cscfgFileName for project $ccProjectName does not include a WebRole/WorkerRole section named $roleName."

    # cscfg update 1 of 1: Remove setting value for client diag level
    $cscfgConfigSettingsNode = $cscfgRoleNode.SelectSingleNode("sd:ConfigurationSettings", $nsMgr)
    if ($cscfgConfigSettingsNode -ne $null)
    {
        $settingsNodes = $cscfgConfigSettingsNode.SelectNodes('sd:Setting[@name="Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel"]', $nsmgr)
        foreach ($settingsNode in $settingsNodes)
        {
	       if ($settingsNode -ne $null)
	       {
                $cscfgConfigSettingsNode.RemoveChild($settingsNode) | Out-Null
	       }
        }
        
        # rkaura: select only element nodes
        $settingsNodes = $cscfgConfigSettingsNode.SelectNodes("*", $nsmgr)
        if ($settingsNodes.Count -eq 0)
        {
	       $cscfgRoleNode.RemoveChild($cscfgConfigSettingsNode) | Out-Null
        }
    }

    $cscfgXml.Save($cscfgFilePath)
}

trap [Exception]
{
    # write error and continue
    Write-Error $_.Exception.ToString()
}
