-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- 主机： localhost
-- 生成日期： 2020-01-24 00:56:35
-- 服务器版本： 5.7.26
-- PHP 版本： 7.3.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- 数据库： `project`
--

-- --------------------------------------------------------

--
-- 表的结构 `admin_info`
--

CREATE TABLE `admin_info` (
  `id` int(11) NOT NULL,
  `name` char(10) COLLATE utf8_unicode_ci DEFAULT NULL,
  `password` char(16) COLLATE utf8_unicode_ci DEFAULT NULL,
  `limit` int(11) DEFAULT NULL,
  `type` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- 转存表中的数据 `admin_info`
--

INSERT INTO `admin_info` (`id`, `name`, `password`, `limit`, `type`) VALUES
(2, 'zjs01', '123456', 0, 4),
(5, 'zjs02', '123456', 1, 4),
(7, 'zjs03', '123456', 2, 4),
(8, 'zjs04', '123456', 3, 0),
(9, 'zjs05', '123456', 3, 1),
(10, 'zjs06', '123456', 3, 2),
(11, 'zjs07', '123456', 3, 3);

-- --------------------------------------------------------

--
-- 表的结构 `commend`
--

CREATE TABLE `commend` (
  `id` int(11) NOT NULL,
  `news_name` char(50) COLLATE utf8_unicode_ci NOT NULL,
  `user_name` char(10) COLLATE utf8_unicode_ci NOT NULL,
  `commend_text` char(100) COLLATE utf8_unicode_ci DEFAULT NULL,
  `time` char(50) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- 转存表中的数据 `commend`
--

INSERT INTO `commend` (`id`, `news_name`, `user_name`, `commend_text`, `time`) VALUES
(1, '“央视频高铁专列”23日启航 将服务春运一线！', 'zzz', '真棒', '2020/1/23 23:54:18'),
(2, '“央视频高铁专列”23日启航 将服务春运一线！', 'zzz', '真的好棒', '2020/1/23 23:55:34');

-- --------------------------------------------------------

--
-- 表的结构 `news`
--

CREATE TABLE `news` (
  `id` int(11) NOT NULL,
  `title` char(255) CHARACTER SET utf8 NOT NULL DEFAULT '',
  `type` int(11) NOT NULL,
  `author` char(50) COLLATE utf8_unicode_ci NOT NULL,
  `time` char(20) COLLATE utf8_unicode_ci NOT NULL,
  `context` char(255) CHARACTER SET utf8 NOT NULL DEFAULT '',
  `picture` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `passed` int(11) NOT NULL,
  `zan` int(11) DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- 转存表中的数据 `news`
--

INSERT INTO `news` (`id`, `title`, `type`, `author`, `time`, `context`, `picture`, `passed`, `zan`) VALUES
(21, '新型冠状病毒疫情的这些问题 武汉市市长予以解答', 0, '新华网', '2020年1月23日', '新华社武汉1月22日电 眼下正值春运高峰，作为疫情核心区的武汉市如何有效管控？被感染患者能否得到及时救治？奋战在一线的医务工作者的健康能否得到保障？\r\n　　针对各界关注的问题，新华社记者就此专访武汉市市长、武汉市新型冠状病毒感染的肺炎疫情防控指挥部指挥长周先旺。', '\\pic\\1579725726871_966.jpg', 1, 1),
(22, '口罩供应如何保量稳价，让老百姓有一道安全防线？', 0, '中国青年报', '2020年1月23日', '“一货架的无纺布口罩，一晚上就卖完了。”安徽界首市华源大药房一位工作人员说。\r\n　　1月22日下午，仅剩的几盒口罩被摆在了药房前台收银处。当记者问及N95口罩时，该工作人员表示没有，春节期间由于供货商放假，无法再进到货。\r\n　　新型冠状病毒疫情遇上春节，口罩在各地告急，电商平台断货，一二线城市部分药房涨价……口罩供应如何保证量足价稳，让老百姓有一道安全防线？', '\\pic\\2020012307261639093.jpg', 1, 1),
(23, '武汉全市公交、地铁、轮渡、长途客运暂停运营', 0, '新华网', '2020年1月23日', '新华社武汉1月23日电（记者冯国栋）武汉市新型冠状病毒感染的肺炎疫情防控指挥部1月23日凌晨发布消息，为全力做好新型冠状病毒感染的肺炎疫情防控工作，有效切断病毒传播途径，坚决遏制疫情蔓延势头，确保人民群众生命安全和身体健康，自1月23日10时起，武汉全市城市公交、地铁、轮渡、长途客运暂停运营；无特殊原因，市民不要离开武汉；机场、火车站离汉通道暂时关闭。恢复时间另行通告。', '\\pic\\11.jpg', 1, 0),
(24, '“央视频高铁专列”23日启航 将服务春运一线！', 0, '中央广播电视台', '2020年1月22日', '“欢迎您乘坐央视频高铁专列……”，伴随着列车播音员轻柔和缓的播报，明天（23日）上午，以“央视频”冠名的高铁列车将加入春运大军。这辆G42次高铁将从杭州东站出发，16节车厢满载上千名旅客驶向目的地北京南站。', '\\pic\\2020012222152211915.jpg', 1, 0),
(25, '美参议院通过特朗普弹劾案审理流程决议', 1, '新华网', '2020年1月22日', '新华社华盛顿1月22日电（记者徐剑梅　邓仙来）经历逾12小时激烈交锋和闭门讨论、11次投票否决民主党提出的修正条款后，共和党掌控的美国国会参议院当地时间22日凌晨投票通过了针对总统特朗普的弹劾案审理流程决议。\r\n　　根据决议，在22日下午开始的开案陈述中，作为控方的众议院民主党“管理人”和作为辩护方的特朗普律师团队各有最多24小时陈述时间，限在3个审理日内使用完毕。', '\\pic\\1579725569696_712.jpg', 1, 0),
(26, '夜探国产首艘万吨级驱逐舰南昌舰', 2, '新华网', '2020年1月22日', '身材魁梧的副航海长段春杰趴在电子海图桌上，眯着眼睛绘制航海计划，身旁放着厚厚一叠已经绘制完毕的海图。\r\n　“没想到用来迎接鼠年春节的，竟然是连续3天通宵加班。”段春杰直了直腰，笑着对记者说，南昌舰的后续训练任务重。“2020年注定是任务接任务、忙碌再忙碌的年份。”', '\\pic\\1579727234684_182.jpg', 1, 0),
(27, '也许会被传染，想多战斗一天', 3, '澎湃新闻', '2020年1月23日', '“我不知道自己哪一天也许就被传染了，但多战斗一天，就能多治疗一个病人。”1月22日晚，汤浩接受澎湃新闻采访时感慨道。\r\n　　汤浩坚定地说：“当医生，不冲前线，谁去冲呢？我们要是躲在老百姓后面，大家要看病，找谁看呢？”\r\n　　当谈及“处于医生的天职和家人的担忧之间的两难”，在医院里乐观鼓励每一位患者的汤浩却一度哽咽，少有地展现了他脆弱的一面。', '\\pic\\2020012305101293366.jpg', 1, 0),
(28, 'zzz', 0, 'zzz', '2020年1月23日', 'sad', 'E:Projects\newWindowsFormsApp1inDebugpic\0.jpg', 0, 0);

-- --------------------------------------------------------

--
-- 表的结构 `news_type`
--

CREATE TABLE `news_type` (
  `id` int(11) NOT NULL,
  `type` char(10) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- 转存表中的数据 `news_type`
--

INSERT INTO `news_type` (`id`, `type`) VALUES
(0, '社会'),
(1, '国际'),
(2, '军事'),
(3, '历史');

-- --------------------------------------------------------

--
-- 表的结构 `user_info`
--

CREATE TABLE `user_info` (
  `id` int(11) NOT NULL,
  `name` char(10) COLLATE utf8_unicode_ci DEFAULT NULL,
  `password` char(16) COLLATE utf8_unicode_ci DEFAULT NULL,
  `sex` char(2) COLLATE utf8_unicode_ci DEFAULT NULL,
  `email` char(30) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- 转存表中的数据 `user_info`
--

INSERT INTO `user_info` (`id`, `name`, `password`, `sex`, `email`) VALUES
(8, 'zzz', '123456', '男', '2993092995@qq.com');

-- --------------------------------------------------------

--
-- 表的结构 `zan`
--

CREATE TABLE `zan` (
  `Id` int(11) NOT NULL,
  `username` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `newsname` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- 转存表中的数据 `zan`
--

INSERT INTO `zan` (`Id`, `username`, `newsname`) VALUES
(1, 'zzz', '口罩供应如何保量稳价，让老百姓有一道安全防线？');

--
-- 转储表的索引
--

--
-- 表的索引 `admin_info`
--
ALTER TABLE `admin_info`
  ADD PRIMARY KEY (`id`);

--
-- 表的索引 `commend`
--
ALTER TABLE `commend`
  ADD PRIMARY KEY (`id`);

--
-- 表的索引 `news`
--
ALTER TABLE `news`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `title` (`title`);

--
-- 表的索引 `news_type`
--
ALTER TABLE `news_type`
  ADD PRIMARY KEY (`id`);

--
-- 表的索引 `user_info`
--
ALTER TABLE `user_info`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `name` (`name`);

--
-- 表的索引 `zan`
--
ALTER TABLE `zan`
  ADD PRIMARY KEY (`Id`);

--
-- 在导出的表使用AUTO_INCREMENT
--

--
-- 使用表AUTO_INCREMENT `admin_info`
--
ALTER TABLE `admin_info`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- 使用表AUTO_INCREMENT `commend`
--
ALTER TABLE `commend`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- 使用表AUTO_INCREMENT `news`
--
ALTER TABLE `news`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- 使用表AUTO_INCREMENT `news_type`
--
ALTER TABLE `news_type`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- 使用表AUTO_INCREMENT `user_info`
--
ALTER TABLE `user_info`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- 使用表AUTO_INCREMENT `zan`
--
ALTER TABLE `zan`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
