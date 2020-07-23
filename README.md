This tool splits double-faced cards exported from [Magic Set Editor](https://magicseteditor.boards.net/) using [the CS/ECH Cockatrice exporter](https://tinyurl.com/szb78x6).

# Usage

1. In MSE, open the File menu and select Export → HTML.
2. Choose the exporter “Cockatrice / Custom Standard 1.04”. (If you can't see the exporters' subtitles, you can check the version by looking at the first export option.)
    * If you don't have the exporter, you can download it from <https://tinyurl.com/szb78x6>.
3. The recommended options are:
    * “Custom” as the Cockatrice Set Type.
    * JPG images.
    * Disable “Tokens In Separate XML” and “Append Set Code To Tokens”.
    * Export all rarities etc.
4. Select where to save your export, and enter the set code as the file name.
5. *(You can skip this step if the set doesn't have any double-faced cards.)* In File Explorer, right-click the exported XML file, and select the downloaded `DfcSplitter.exe` from the “Open with…” menu.
    * The first time you do this, you will first have to download `DfcSplitter.exe` from <https://github.com/fenhl/dfc-splitter/releases/latest>. Then in the “Open with…” menu, click through “Choose another app”, then “More apps”, then “Look for another app on this PC”, then navigate to your Downloads folder.
6. If you want to send the file to others, do that now, and tell your players the following steps.
7. In Cockatrice, open the Card Database menu and select Open custom image folder.
8. Move the folder with the images into the custom image folder.
9. In Cockatrice, open the Card Database menu and select Open custom sets folder.
10. Move the XML file into the custom sets folder.
11. Restart Cockatrice.
