# Install.ps1
param($installPath, $toolsPath, $package, $project)

Function Clean-Dups-In-Config-File ($localPath, $parentNodePath, $nodePath)
{
	# load config as XML
	$xml.Load($localPath)

	# select parent node to call the RemoveChild on later
	$parentNode = $xml.SelectSingleNode($parentNodePath)

	# select the nodes
	$nodes = $xml.SelectNodes($parentNodePath + "/" + $nodePath)

	if ($nodes.Count -gt 0) 
	{
		# if there are multiple repository names with same key, then the NuGet update added the additional ones and they will be last
		#	so loop through those nodes, excluding the first one, and remove them
		$i = 1;
		while ($i -lt $nodes.Count) 
		{
			$parentNode.RemoveChild($nodes.Item($i))
			$i++
		}

		# save the config file
		$xml.Save($localPath)
	}

}

$xml = New-Object xml

# find the Web.config file if there is one
$config = $project.ProjectItems | where {$_.Name -eq "Web.config" }
if ($config -ne $null) 
{
	# find its path on the file system
	$localPath = $config.Properties | where {$_.Name -eq "LocalPath"}
	
	# clean duplicate entries
	Clean-Dups-In-Config-File $localPath.Value "configuration/sharpRepository/cachingStrategies" "cachingStrategy[@name='standardCachingStrategy']"
	Clean-Dups-In-Config-File $localPath.Value "configuration/sharpRepository/cachingStrategies" "cachingStrategy[@name='timeoutCachingStrategy']"
	Clean-Dups-In-Config-File $localPath.Value "configuration/sharpRepository/cachingStrategies" "cachingStrategy[@name='noCaching']"
}

# find the App.config file if there is one
$config = $project.ProjectItems | where {$_.Name -eq "App.config" }
if ($config -ne $null) 
{
	# find its path on the file system
	$localPath = $config.Properties | where {$_.Name -eq "LocalPath"}
	
	# clean duplicate entries
	Clean-Dups-In-Config-File $localPath.Value "configuration/sharpRepository/cachingStrategies" "cachingStrategy[@name='standardCachingStrategy']"
	Clean-Dups-In-Config-File $localPath.Value "configuration/sharpRepository/cachingStrategies" "cachingStrategy[@name='timeoutCachingStrategy']"
	Clean-Dups-In-Config-File $localPath.Value "configuration/sharpRepository/cachingStrategies" "cachingStrategy[@name='noCaching']"
}

