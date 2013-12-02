# Install.ps1
param($installPath, $toolsPath, $package, $project)

# Settings
$parentPath = "configuration/sharpRepository/repositories"
$keyPath = "repository[@name='efRepository']"

Function Clean-Dups-In-Config-File ($localPath, $parentNodePath, $nodePath)
{
	$xml = New-Object xml

	# load config as XML
	$xml.Load($localPath)

	# select parent node to call the RemoveChild on later
	$parentNode = $xml.SelectSingleNode($parentNodePath)

	# select the nodes
	$nodes = $xml.SelectNodes($parentNodePath + "/" + $nodePath)

	if ($nodes.Count -gt 0) 
	{
		# if there are multiple repository names with the same key, then the NuGet update added the additional ones and they will be last
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

# find the Web.config/App.config file if there is one
$items = $project.ProjectItems | where {$_.Name -eq "Web.config" -or $_.Name -eq "App.config" }
Foreach ($item in $items)
{
	# find its path on the file system
	$localPath = $item.Properties | where {$_.Name -eq "LocalPath"}
	
	# clean duplicate entries
	Clean-Dups-In-Config-File $localPath.Value $parentPath $keyPath
}
