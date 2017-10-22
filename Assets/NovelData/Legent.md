规则

- %chat [event] 一段操作的开始，event为触发这段操作的事件，%end代表结束
  - gamestart 游戏开始
  - click [btn_name] 按下某个按钮
- %end 代表上一段操作结束，同时关闭所有浮动对话，例如画外音
- %panel [panel_name] \[func_name] \[args] 对名为panel_name的界面执行func_name函数，args为输入的参数
- %sleep [num] 代表等待num秒之后再继续进行后续的对话，不需要%end
- 非N结尾的仅首字母大写的单词：单词代表人物编号，与此人进行短信聊天，如果不在短信界面，将强制跳转至短信界面
  - M:可以在人物表中查看人物详细信息
- 以N结尾的仅首字母大写的单词：去掉末尾的N的单词代表人物编号，与此人进行画外音聊天，任何界面均可进行
  - MN:即为M
- 非%或人名+冒号开头的，是为注释，与游戏逻辑无关
- 聊天：在人物名称后边可以用()来标注此时的额外操作
  - focus=[name] 高亮名为name的控件
  - wait=[event] 当事件发生后才进行后续的对话，而不是默认的点击事件
  - refresh=[event] 当事件发生时，更新当前的focus
  - arrowto=[btn_name] 显示指向 btn_name 的箭头，并轻微高亮显示btn_name
  - screenshake=[true|false] 进行一次屏幕抖动
- []在行首：代表此处为分支对话，每个分支内容置于一个中括号内部，互相相连，可嵌套
- %cinema 打开过场动画界面，可带参数
  - pic=[file_name] 播放名为 file_name 的图片
  - txt="content" 播放引号内的文本，可换行，如果换行，那么第二个引号需要独占一行
  - txt_speed=[num] 文字显示速度
  - txt_type=[line|char] 每次显示一行或者一个字
  - time=[num] 播放图片或文本至少n秒，再执行后续的渐出
  - fadein=[num] 播放图片或文本结束后，等待n秒渐入，默认为0.5
  - fadeout=[num] 播放图片或文本结束后，等待n秒渐出，默认为1
  - anim=[anim_type] 对图片执行的动画操作
  - anim_speed=[num] 动画播放速度


# name

> 人物具体信息在人物表中配置

- S: 玩家自己
- M:新手引导人

# event

>多个事件可以通过 | 符号连接起来，详细内容在事件表配置

- click [btn_name]: 点击 btn_name
- new_note_people: 添加了新笔记，内容为人
- new_note_item:添加了新笔记，内容为物
- no_new_note_item:笔记中的所有物品都已经被检视过

panel_name

- panel_note: 笔记本界面