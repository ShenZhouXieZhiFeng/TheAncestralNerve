# TheAncestralNerve
这是一条祖传的神经网络
===
基于神经网络和遗传算法的unity开发框架，可以轻易的应用到各种不同类型的游戏中
---
后续任务：<br>
  1.降低使用门槛，完善各类API，制作各类demo<br>
  2.扩展遗传算法，添加可配置的遗传参数，包括交换概率，突变概率，筛选方式，交叉方式，突变方式等<br>
  3....<br>

  20171226 完成第一个demo，坦克扫雷<br>
  ![](https://github.com/ShenZhouXieZhiFeng/TheAncestralNerve/blob/master/Images/demo.png?raw=true)
  
  核心代码参考：https://github.com/ArztSamuel/Applying_EANNs
  参考书籍：游戏编程中的人工智能技术（详细的介绍了神经网络和遗传算法在游戏领域的应用，虽然有点老，但不失为一本很好的入门书籍）
  
  # EasyAIFrame使用说明
  ## 使用介绍：
  ### 1.管理类
  #### 1.1预制体
  ![](https://github.com/ShenZhouXieZhiFeng/TheAncestralNerve/blob/master/Images/intro/%E5%9B%BE%E7%89%871.png)<br>
  将AIManager拖拽到场景中，构建自己的游戏管理类，如demo中的TanksManager脚本<br>
  #### 1.2使用代码开始遗传进程
  ![](https://github.com/ShenZhouXieZhiFeng/TheAncestralNerve/blob/master/Images/intro/%E5%9B%BE%E7%89%872.png)<br>
  #### 1.3一些回调
  ![](https://github.com/ShenZhouXieZhiFeng/TheAncestralNerve/blob/master/Images/intro/%E5%9B%BE%E7%89%873.png)<br>
  直接设置<br>
  ![](https://github.com/ShenZhouXieZhiFeng/TheAncestralNerve/blob/master/Images/intro/%E5%9B%BE%E7%89%874.png)<br>
  ### 2.实体类
  #### 2.1创建自己的实体
  ![](https://github.com/ShenZhouXieZhiFeng/TheAncestralNerve/blob/master/Images/intro/%E5%9B%BE%E7%89%875.png)<br>
  创建自己的游戏实体类，并继承自Entity类
  同时需要实现一些方法
  ![](https://github.com/ShenZhouXieZhiFeng/TheAncestralNerve/blob/master/Images/intro/%E5%9B%BE%E7%89%876.png)<br>
  #### 2.2实体的配置与应用
  关键函数是SetInputs和GetOutPuts<br>
  需要在这两个函数中配置游戏对象的输入和输出，输入可以认为是智能判断自身处境的条件，输入即是智能判断的结果<br>
  ![](https://github.com/ShenZhouXieZhiFeng/TheAncestralNerve/blob/master/Images/intro/%E5%9B%BE%E7%89%877.png)<br>
  以demo中的坦克为例，我设置了4个输入和2个输出<br>
  4个输入分别为当前tank的速度，旋转，最近的地雷方位（让智能能够辨识最近的地雷在哪），最近的tank方位（防止多辆tank互相挤在一堆）<br>
  2个输出为tank的目标速度和位移，用于控制tank做出下一步动作<br>
  #### 2.3行为奖励
  判断哪些行为是正确的，哪些是错误的，主要是通过分数加减的形式<br>
  在达成正确行为时可以增加实体的分数<br>
  以demo为例：<br>
  奖励机制为当tank吃到一个地雷时就增加它的分数<br>
  惩罚机制为当两个tank互相碰撞时就减少他们的分数<br>
  ![](https://github.com/ShenZhouXieZhiFeng/TheAncestralNerve/blob/master/Images/intro/%E5%9B%BE%E7%89%878.png)<br>
  ### 3.参数配置
  遗传参数的配置，选中管理类预制体，打开下图窗口即可编辑遗传和神经网络的相关参数，这里设置的输入参数和输出参数必须和实际相等<br>
  ![](https://github.com/ShenZhouXieZhiFeng/TheAncestralNerve/blob/master/Images/intro/%E5%9B%BE%E7%89%879.png)<br>
  ### 4.高级扩展，自定义选择，变异和交叉策略
  如果你对与遗传算法有一定的了解，可以根据需要改变以上三中策略<br>
  打开GeneticAlgorithm.GeneticOpetation脚本<br>
  ![](https://github.com/ShenZhouXieZhiFeng/TheAncestralNerve/blob/master/Images/intro/%E5%9B%BE%E7%89%8710.png)<br>
  可以看到这三种策略的方法是用委托绑定的，实现你自己的策略函数，直接修改委托的绑定对象，即可生效<br>
  ![](https://github.com/ShenZhouXieZhiFeng/TheAncestralNerve/blob/master/Images/intro/%E5%9B%BE%E7%89%8711.png)<br>
  
  
  
