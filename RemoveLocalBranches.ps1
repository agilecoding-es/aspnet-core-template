$OutputEncoding = [Console]::OutputEncoding

$branches = $(git branch -vv | findstr " gone]")

if($branches){
    $branches.trim() | ForEach-Object -Process {
        git branch -D ($_).split(" ")[0]
    }
} 

pause