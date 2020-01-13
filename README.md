This tool splits double-faced cards exported from [Magic Set Editor](https://magicseteditor.boards.net/) using the Custom Standard Cockatrice exporter, which you can find pinned in [#format-design](https://discord.gg/gKJhAb6) in the Custom Standard Discord server.

# Usage

1. In MSE, open the File menu and select Export → HTML.
2. Choose the exporter “Cockatrice / Custom Standard 1.03.FH6”. (If you can't see the exporters' subtitles, you can check the version by looking at the first export option.)
3. The recommended options are:
    * “Custom” as the Cockatrice Set Type.
    * JPG images.
    * Disable “Tokens In Separate XML” and “Append Set Code To Tokens”.
    * Export all rarities etc.
4. Select where to save your export, and enter the set code as the file name.
5. If the set doesn't have any double-faced cards, skip to step 13.
6. In File Explorer, find the `DfcSplitter.exe` downloaded from <https://github.com/fenhl/dfc-splitter/releases/latest>, hold shift, right-click it, and select “Copy as path”.
7. In File Explorer, navigate to the folder where the exported XML file and images folder are located.
8. Hold shift and right-click the File Explorer window, and select “Open PowerShell window here”.
9. Right-click the PowerShell window to paste the full path to the DFC splitter.
10. Remove the `"` both at the start and at the end.
11. At the end, add a space, followed by the set code.
12. Hit return. You can close PowerShell after it says “done!”
13. If you want to send the file to others, do that now, and tell your players the following steps.
14. In Cockatrice, open the Card Database menu and select Open custom image folder.
15. Move the folder with the images into the custom image folder.
16. In Cockatrice, open the Card Database menu and select Open custom sets folder.
17. Move the XML file into the custom sets folder.
18. Restart Cockatrice.
