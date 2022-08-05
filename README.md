# UpdateWaterFM
更新機制說明：

會從FTP 164 上取得相關資料
updateConfig.json決定目前版本 降版的話 就會從版號1開始重新跑一輪

fileMapping.json對上更新檔案的位置 預設在C:/Ximple/WaterFM底下
如果有例外直接打全部路徑(ex. C:\edmini\Arfluex.exe)

加上新版本的方式：
在ftp://10.68.127.164/WaterFMUpdate/ 加上新的資料夾 名稱為數字依序向上加
如果有filemapping沒有的就要加進去


**FTP不允許excel等副檔名 所以先改成txt然後再改回來即可