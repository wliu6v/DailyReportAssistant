DailyReportAssistant
====================

**TODO:**

-	Add File Encoding Detact
-	key up & key down
-	Setting -- ask for svn address / username / password / ...
-	UI -- Add window border

Update 20131213
--------------
-	能够自动生成 svn commit 的脚本了。不过其实不生成应该也无所谓
-	

Update 20131212
--------------
修改了许多内容：  
-	每行日报填写完毕的时候都检查一下末尾字符是否为句号"。"，如果不是的话则添加
-	修复了之前在1000字节位置出现乱码的问题
-	优化了对于换行符的处理方式，尽量避免跨平台问题。
-	添加了对文件编码的选择
-	添加了选择是否svn提交的功能

Update 20131211
---------------
-	修改了 App.config，添加了一些新的设置项。

Update 20131210
----------------

-	添加了 App.config 及相关的一些处理方式，现在可以通过配置文件读写 日报文件路径 了。
