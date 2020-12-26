open System.Diagnostics
open System.IO

[<EntryPoint>]
let main argv =
    printfn "Drawing a tree"
    
    let outfolder = @"C:\code\FractalTrees\FractalTrees\results"
    if Directory.Exists outfolder |> not then Directory.CreateDirectory outfolder |> ignore
    let outPath = Path.Combine(outfolder, "tree.jpg")
    
    Tree.drawTree outPath
    
    Process.Start(ProcessStartInfo(outPath, UseShellExecute = true)) 
    |> ignore
    
    0 // return an integer exit code
