-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- 主机： localhost
-- 生成日期： 2020-01-10 21:08:33
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
  `limit` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- 转存表中的数据 `admin_info`
--

INSERT INTO `admin_info` (`id`, `name`, `password`, `limit`) VALUES
(1, 'zjs', '123456', 1),
(2, 'zjs01', '123456', 0);

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
(1, 'zzz', 'zzz', '挺好', '2020年1月10日'),
(2, 'zzz', 'zzz', '无', '2020年1月10日'),
(3, 'zzz', 'zzz', 's', '2020年1月10日'),
(4, 'zzzz', 'zzz', '无', '2020年1月10日'),
(5, 'zzzz', 'zzz', '无1', '2020年1月10日');

-- --------------------------------------------------------

--
-- 表的结构 `news`
--

CREATE TABLE `news` (
  `id` int(11) NOT NULL,
  `title` char(50) COLLATE utf8_unicode_ci NOT NULL,
  `type` int(11) NOT NULL,
  `author` char(50) COLLATE utf8_unicode_ci NOT NULL,
  `time` char(20) COLLATE utf8_unicode_ci NOT NULL,
  `context` char(50) COLLATE utf8_unicode_ci NOT NULL,
  `passed` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- 转存表中的数据 `news`
--

INSERT INTO `news` (`id`, `title`, `type`, `author`, `time`, `context`, `passed`) VALUES
(9, 'zzz', 0, 'zzzz', '2020年1月8日', 'zhjbadcenf', 1),
(10, '特朗普回应美军驻地遭袭后 这句话登上推特热搜', 1, '观察者网', '2020年1月8日', '原标题：特朗普回应美军驻地遭袭后，“all is well”登上推特热搜', 1),
(12, 'z', 2, 'zzzzz', '2020年1月8日', 'asdsfd', 1),
(15, 'zzzz', 0, 'zzz', '2020年1月8日', 'sdcsd', 1);

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
-- 在导出的表使用AUTO_INCREMENT
--

--
-- 使用表AUTO_INCREMENT `admin_info`
--
ALTER TABLE `admin_info`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- 使用表AUTO_INCREMENT `commend`
--
ALTER TABLE `commend`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- 使用表AUTO_INCREMENT `news`
--
ALTER TABLE `news`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

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
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
