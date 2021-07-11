# xiaoquduo
校趣多打卡，基于.NET Core3.1的控制台应用程序，无框体方便用于计划任务达到每日循环，支持多账号，支持log查阅

## 环境/工具

环境：.NET Core3.1 <br />
取cookie用的工具： Fiddler2，PC版微信<br />

### 面向萌新的操作

* 初次运行程序
    1. 下载Releases 已经打包好的压缩包<br />
    2. 用记事本修改压缩包解压后文件夹里的**数据.ini文件**<br />
    3. 运行校趣多打卡.exe<br />
    
* 查找cookie
    1. 打开电脑微信，进小程序前打开Fiddler2<br />
    2. Fiddler2有显示抓到包的数据后，打开校趣多<br />
    3. 依次点击 疫情防控-健康打卡<br />
    4. 找到如下图这一行（Result302的下面一行，URL是/corona/submitHealthCheck?openId)，复制URL中openId=后到&latitude=&longitude= 之前的的字符串(一共96个字符)
![找到openid](https://www.z4a.net/images/2021/07/11/2021-07-11_22-04-26.jpg)
    5. 如上图复制的则是20b21a0a4~省略~4dc12505<br />
    6. 将复制的填入到**数据.ini文件**的openid栏<br />

### ini参数解释
节点名称\[test]下的均为例子，不会运行，因此无需修改<br />
需要修改的是\[1]下的参数，如果想加多个账号，就类似的在下面复制粘贴就好了<br />
|  键   | 值  |
|  ----  | ----  |
| openid  | 应该是由96个字母或数字组成 |
| CheckPlace  | 打卡地点 |
| Temperature  | 当前体温 |
| Phone  | 联系方式 |
| livingPlace  | 现居住地 |
| livingPlaceDetail  | 详细地址 |
| checkPlaceProvince(定位用)  | 定位到的省 |
| checkPlaceCity(定位用)  | 定位到的城 |
| checkPlaceArea(定位用)  | 定位到的区 |

### 注意事项

* 程序在运行成功后3s自动关闭，是否成功如没看到可以去查看生成的log文件，这么做是为了方便丢到windows自带的计划任务里做到每日循环，怎么做？请百度windows计划任务<br />
* 多账户的节点名称可以随意命名，本质是集合遍历
* **感谢[FengZzhi](https://github.com/FengZzhi)在issues里的建议，改用openid（后面会说明）后成功实现每日全自动打卡**
* 填写计划任务时请注意 起始于 务必填写到 校趣多打卡.exe 的更目录（如图）
![计划任务](https://www.z4a.net/images/2020/10/27/2020-10-27_21-16-52.png)

### 出错详解

* 打开就闪退说明没有运行环境，请百度安装.NET Core3.1环境<br />
* 如显示的是未知错误，log里写的是cookie出错，基本就是openid输错了<br />
* 如果显示联系方式出错，则是Phone栏输入有误<br />
* 为什么我输的都正确返回还是错误的？先检查是不是写到test里去了，第一个用户请写在\[1]里<br />

### 已知但无法解决的bug
* 因为本地断网无法发出请求，即收到回复为空，和成功打卡时收到回复为空一致，会导致 成功打卡 的误判断

### 思路/博客/其他
* python版本请参看[python](https://github.com/FengZzhi/xiaoquduo)
* 思路等见[博客](https://hd80606b.com/xiaoquduo/)<br />
**如果你觉得可以的话，给个Star吧**

### 关于openid
* 单独起一栏说说这openid，这里的openid并不是指的**微信用户在公众号appid下的唯一用户标识**所用的openid和UnionId，详情参见微信小程序开发文档[文档](https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=4_4) 
* 这里的openid虽说每次都有所变动，但竟然会一直保留信息，并且可以一直用于set-cookie，之前实则没有想到，正是这个原因，也要谨防泄露这串字符串，有了这串字符串就可以很容易调出上次打卡所填写的信息
