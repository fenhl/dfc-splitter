This tool splits double-faced cards exported from [Magic Set Editor](https://magicseteditor.boards.net/) using [the CS/ECH Cockatrice exporter](https://tinyurl.com/szb78x6).

# Usage

1. Download `DfcSplitter.exe` from <https://github.com/fenhl/dfc-splitter/releases/latest>.
2. In MSE, open the File menu and select Export → HTML.
3. Choose the exporter “Cockatrice / Custom Standard 1.04”. (If you can't see the exporters' subtitles, you can check the version by looking at the first export option.)
    * If you don't have the exporter, you can download it from <https://tinyurl.com/szb78x6>.
4. The recommended options are:
    * “Custom” as the Cockatrice Set Type.
    * JPG images.
    * Disable “Tokens In Separate XML” and “Append Set Code To Tokens”.
    * Export all rarities etc.
5. Select where to save your export, and enter the set code as the file name.
6. If the set doesn't have any double-faced cards, skip to step 8.
7. In File Explorer, right-click the exported XML file, and select the downloaded `DfcSplitter.exe` from the “Open with…” menu.
    * The first time you do this, you will have to click through “Choose another app”, then “More apps”, then “Look for another app on this PC”, then navigate to your Downloads folder.
8. If you want to send the file to others, do that now, and tell your players the following steps.
9. In Cockatrice, open the Card Database menu and select Open custom image folder.
10. Move the folder with the images into the custom image folder.
11. In Cockatrice, open the Card Database menu and select Open custom sets folder.
12. Move the XML file into the custom sets folder.
13. Restart Cockatrice.
