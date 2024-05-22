-- MySQL dump 10.13  Distrib 8.0.22, for Win64 (x86_64)
--
-- Host: 104.225.208.163    Database: Deadit_Dev
-- ------------------------------------------------------
-- Server version	8.0.36-0ubuntu0.20.04.1

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `Deadit_Dev`
--

USE `Deadit_Dev`;

--
-- Table structure for table `Api_Access_Token`
--

DROP TABLE IF EXISTS `Api_Access_Token`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Api_Access_Token` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `token` char(36) NOT NULL,
  `user_id` int unsigned NOT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `token` (`token`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `Api_Access_Token_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `User` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Banned_Community_Name`
--

DROP TABLE IF EXISTS `Banned_Community_Name`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Banned_Community_Name` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` char(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=639 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Comment`
--

DROP TABLE IF EXISTS `Comment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Comment` (
  `id` char(36) NOT NULL,
  `author_id` int unsigned NOT NULL,
  `post_id` char(36) NOT NULL,
  `content` text NOT NULL,
  `parent_id` char(36) DEFAULT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `deleted_on` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `author_id` (`author_id`),
  KEY `post_id` (`post_id`),
  KEY `parent_id` (`parent_id`),
  CONSTRAINT `Comment_ibfk_1` FOREIGN KEY (`author_id`) REFERENCES `User` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `Comment_ibfk_2` FOREIGN KEY (`post_id`) REFERENCES `Post` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Comment_ibfk_3` FOREIGN KEY (`parent_id`) REFERENCES `Comment` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Community`
--

DROP TABLE IF EXISTS `Community`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Community` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` char(50) NOT NULL,
  `title` char(100) NOT NULL,
  `owner_id` int unsigned NOT NULL,
  `description` char(200) DEFAULT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `name` (`name`),
  KEY `owner_id` (`owner_id`),
  CONSTRAINT `Community_ibfk_1` FOREIGN KEY (`owner_id`) REFERENCES `User` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Community_Membership`
--

DROP TABLE IF EXISTS `Community_Membership`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Community_Membership` (
  `community_id` int unsigned NOT NULL,
  `user_id` int unsigned NOT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`community_id`,`user_id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `Community_Membership_ibfk_1` FOREIGN KEY (`community_id`) REFERENCES `Community` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Community_Membership_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `User` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Error_Message`
--

DROP TABLE IF EXISTS `Error_Message`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Error_Message` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `error_message_group_id` int unsigned NOT NULL,
  `message` varchar(250) NOT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `error_message_group_id` (`error_message_group_id`),
  CONSTRAINT `Error_Message_ibfk_1` FOREIGN KEY (`error_message_group_id`) REFERENCES `Error_Message_Group` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=402 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Error_Message_Group`
--

DROP TABLE IF EXISTS `Error_Message_Group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Error_Message_Group` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Post`
--

DROP TABLE IF EXISTS `Post`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Post` (
  `id` char(36) NOT NULL,
  `community_id` int unsigned NOT NULL,
  `title` char(250) NOT NULL,
  `post_type` smallint unsigned NOT NULL,
  `author_id` int unsigned NOT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `deleted_on` timestamp NULL DEFAULT NULL,
  `archived_on` timestamp NULL DEFAULT NULL,
  `mod_removed_on` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `community_id` (`community_id`),
  KEY `post_type` (`post_type`),
  KEY `author_id` (`author_id`),
  CONSTRAINT `Post_ibfk_1` FOREIGN KEY (`community_id`) REFERENCES `Community` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `Post_ibfk_2` FOREIGN KEY (`post_type`) REFERENCES `Post_Type` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `Post_ibfk_3` FOREIGN KEY (`author_id`) REFERENCES `User` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Post_Link`
--

DROP TABLE IF EXISTS `Post_Link`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Post_Link` (
  `id` char(36) NOT NULL,
  `url` text NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  CONSTRAINT `Post_Link_ibfk_1` FOREIGN KEY (`id`) REFERENCES `Post` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Post_Text`
--

DROP TABLE IF EXISTS `Post_Text`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Post_Text` (
  `id` char(36) NOT NULL,
  `content` text,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  CONSTRAINT `Post_Text_ibfk_1` FOREIGN KEY (`id`) REFERENCES `Post` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Post_Type`
--

DROP TABLE IF EXISTS `Post_Type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Post_Type` (
  `id` smallint unsigned NOT NULL AUTO_INCREMENT,
  `name` char(50) NOT NULL,
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `User`
--

DROP TABLE IF EXISTS `User`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `User` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `email` char(200) NOT NULL,
  `username` char(30) NOT NULL,
  `password` char(250) NOT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `email` (`email`),
  UNIQUE KEY `username` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=121 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Temporary view structure for view `View_Api_Access_Token`
--

DROP TABLE IF EXISTS `View_Api_Access_Token`;
/*!50001 DROP VIEW IF EXISTS `View_Api_Access_Token`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `View_Api_Access_Token` AS SELECT 
 1 AS `token_id`,
 1 AS `token_token`,
 1 AS `token_user_id`,
 1 AS `token_created_on`,
 1 AS `user_id`,
 1 AS `user_email`,
 1 AS `user_username`,
 1 AS `user_password`,
 1 AS `user_created_on`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `View_Comment`
--

DROP TABLE IF EXISTS `View_Comment`;
/*!50001 DROP VIEW IF EXISTS `View_Comment`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `View_Comment` AS SELECT 
 1 AS `comment_id`,
 1 AS `comment_author_id`,
 1 AS `comment_post_id`,
 1 AS `comment_content`,
 1 AS `comment_parent_id`,
 1 AS `comment_created_on`,
 1 AS `comment_deleted_on`,
 1 AS `comment_count_votes_up`,
 1 AS `comment_count_votes_down`,
 1 AS `comment_count_votes_none`,
 1 AS `comment_count_votes_score`,
 1 AS `comment_author_username`,
 1 AS `community_id`,
 1 AS `community_name`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `View_Comment_Vote_Totals`
--

DROP TABLE IF EXISTS `View_Comment_Vote_Totals`;
/*!50001 DROP VIEW IF EXISTS `View_Comment_Vote_Totals`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `View_Comment_Vote_Totals` AS SELECT 
 1 AS `comment_id`,
 1 AS `count_novotes`,
 1 AS `count_downvotes`,
 1 AS `count_upvotes`,
 1 AS `score`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `View_Community`
--

DROP TABLE IF EXISTS `View_Community`;
/*!50001 DROP VIEW IF EXISTS `View_Community`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `View_Community` AS SELECT 
 1 AS `community_id`,
 1 AS `community_name`,
 1 AS `community_title`,
 1 AS `community_owner_id`,
 1 AS `community_description`,
 1 AS `community_created_on`,
 1 AS `community_count_members`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `View_Community_Membership`
--

DROP TABLE IF EXISTS `View_Community_Membership`;
/*!50001 DROP VIEW IF EXISTS `View_Community_Membership`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `View_Community_Membership` AS SELECT 
 1 AS `community_id`,
 1 AS `community_name`,
 1 AS `community_title`,
 1 AS `community_owner_id`,
 1 AS `community_description`,
 1 AS `community_created_on`,
 1 AS `community_count_members`,
 1 AS `user_id`,
 1 AS `user_email`,
 1 AS `user_username`,
 1 AS `user_password`,
 1 AS `user_created_on`,
 1 AS `user_joined_community_on`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `View_Error_Message`
--

DROP TABLE IF EXISTS `View_Error_Message`;
/*!50001 DROP VIEW IF EXISTS `View_Error_Message`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `View_Error_Message` AS SELECT 
 1 AS `error_message_id`,
 1 AS `error_message_group_name`,
 1 AS `error_message_message`,
 1 AS `error_message_created_on`,
 1 AS `error_message_group_id`,
 1 AS `error_message_group_created_on`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `View_Post`
--

DROP TABLE IF EXISTS `View_Post`;
/*!50001 DROP VIEW IF EXISTS `View_Post`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `View_Post` AS SELECT 
 1 AS `post_id`,
 1 AS `post_community_id`,
 1 AS `post_title`,
 1 AS `post_post_type`,
 1 AS `post_author_id`,
 1 AS `post_created_on`,
 1 AS `post_deleted_on`,
 1 AS `post_archived_on`,
 1 AS `post_mod_removed_on`,
 1 AS `post_count_comments`,
 1 AS `community_id`,
 1 AS `community_name`,
 1 AS `community_title`,
 1 AS `community_owner_id`,
 1 AS `community_description`,
 1 AS `community_created_on`,
 1 AS `community_count_members`,
 1 AS `user_id`,
 1 AS `user_email`,
 1 AS `user_username`,
 1 AS `user_password`,
 1 AS `user_created_on`,
 1 AS `post_count_votes_upvotes`,
 1 AS `post_count_votes_downvotes`,
 1 AS `post_count_votes_novotes`,
 1 AS `post_count_votes_score`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `View_Post_Link`
--

DROP TABLE IF EXISTS `View_Post_Link`;
/*!50001 DROP VIEW IF EXISTS `View_Post_Link`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `View_Post_Link` AS SELECT 
 1 AS `post_url`,
 1 AS `post_id`,
 1 AS `post_community_id`,
 1 AS `post_title`,
 1 AS `post_post_type`,
 1 AS `post_author_id`,
 1 AS `post_created_on`,
 1 AS `post_deleted_on`,
 1 AS `post_archived_on`,
 1 AS `post_mod_removed_on`,
 1 AS `post_count_comments`,
 1 AS `community_id`,
 1 AS `community_name`,
 1 AS `community_title`,
 1 AS `community_owner_id`,
 1 AS `community_description`,
 1 AS `community_created_on`,
 1 AS `community_count_members`,
 1 AS `user_id`,
 1 AS `user_email`,
 1 AS `user_username`,
 1 AS `user_password`,
 1 AS `user_created_on`,
 1 AS `post_count_votes_upvotes`,
 1 AS `post_count_votes_downvotes`,
 1 AS `post_count_votes_novotes`,
 1 AS `post_count_votes_score`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `View_Post_Text`
--

DROP TABLE IF EXISTS `View_Post_Text`;
/*!50001 DROP VIEW IF EXISTS `View_Post_Text`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `View_Post_Text` AS SELECT 
 1 AS `post_content`,
 1 AS `post_id`,
 1 AS `post_community_id`,
 1 AS `post_title`,
 1 AS `post_post_type`,
 1 AS `post_author_id`,
 1 AS `post_created_on`,
 1 AS `post_deleted_on`,
 1 AS `post_archived_on`,
 1 AS `post_mod_removed_on`,
 1 AS `post_count_comments`,
 1 AS `community_id`,
 1 AS `community_name`,
 1 AS `community_title`,
 1 AS `community_owner_id`,
 1 AS `community_description`,
 1 AS `community_created_on`,
 1 AS `community_count_members`,
 1 AS `user_id`,
 1 AS `user_email`,
 1 AS `user_username`,
 1 AS `user_password`,
 1 AS `user_created_on`,
 1 AS `post_count_votes_upvotes`,
 1 AS `post_count_votes_downvotes`,
 1 AS `post_count_votes_novotes`,
 1 AS `post_count_votes_score`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `View_Post_Vote_Totals`
--

DROP TABLE IF EXISTS `View_Post_Vote_Totals`;
/*!50001 DROP VIEW IF EXISTS `View_Post_Vote_Totals`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `View_Post_Vote_Totals` AS SELECT 
 1 AS `post_id`,
 1 AS `count_novotes`,
 1 AS `count_downvotes`,
 1 AS `count_upvotes`,
 1 AS `score`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `View_User`
--

DROP TABLE IF EXISTS `View_User`;
/*!50001 DROP VIEW IF EXISTS `View_User`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `View_User` AS SELECT 
 1 AS `user_id`,
 1 AS `user_email`,
 1 AS `user_username`,
 1 AS `user_password`,
 1 AS `user_created_on`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `View_Vote_Comment`
--

DROP TABLE IF EXISTS `View_Vote_Comment`;
/*!50001 DROP VIEW IF EXISTS `View_Vote_Comment`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `View_Vote_Comment` AS SELECT 
 1 AS `vote_comment_comment_id`,
 1 AS `vote_comment_user_id`,
 1 AS `vote_comment_vote_type`,
 1 AS `vote_comment_created_on`,
 1 AS `vote_comment_username`,
 1 AS `post_id`,
 1 AS `comment_count_votes_up`,
 1 AS `comment_count_votes_down`,
 1 AS `comment_count_votes_none`,
 1 AS `comment_count_votes_score`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `View_Vote_Post`
--

DROP TABLE IF EXISTS `View_Vote_Post`;
/*!50001 DROP VIEW IF EXISTS `View_Vote_Post`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `View_Vote_Post` AS SELECT 
 1 AS `vote_post_post_id`,
 1 AS `vote_post_user_id`,
 1 AS `vote_post_vote_type`,
 1 AS `vote_post_created_on`,
 1 AS `vote_post_user_username`,
 1 AS `post_community_id`,
 1 AS `post_community_name`,
 1 AS `post_post_type`,
 1 AS `post_count_votes_upvotes`,
 1 AS `post_count_votes_downvotes`,
 1 AS `post_count_votes_novotes`,
 1 AS `post_count_votes_score`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `Vote_Comment`
--

DROP TABLE IF EXISTS `Vote_Comment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Vote_Comment` (
  `comment_id` char(36) NOT NULL,
  `user_id` int unsigned NOT NULL,
  `vote_type` smallint unsigned NOT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`comment_id`,`user_id`),
  KEY `user_id` (`user_id`),
  KEY `vote_type` (`vote_type`),
  CONSTRAINT `Vote_Comment_ibfk_1` FOREIGN KEY (`comment_id`) REFERENCES `Comment` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Vote_Comment_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `User` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Vote_Comment_ibfk_3` FOREIGN KEY (`vote_type`) REFERENCES `Vote_Type` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Vote_Post`
--

DROP TABLE IF EXISTS `Vote_Post`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Vote_Post` (
  `post_id` char(36) NOT NULL,
  `user_id` int unsigned NOT NULL,
  `vote_type` smallint unsigned NOT NULL,
  `created_on` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`post_id`,`user_id`),
  KEY `user_id` (`user_id`),
  KEY `vote_type` (`vote_type`),
  CONSTRAINT `Vote_Post_ibfk_1` FOREIGN KEY (`post_id`) REFERENCES `Post` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Vote_Post_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `User` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `Vote_Post_ibfk_3` FOREIGN KEY (`vote_type`) REFERENCES `Vote_Type` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Vote_Type`
--

DROP TABLE IF EXISTS `Vote_Type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Vote_Type` (
  `id` smallint unsigned NOT NULL AUTO_INCREMENT,
  `name` char(20) NOT NULL,
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping events for database 'Deadit_Dev'
--

--
-- Dumping routines for database 'Deadit_Dev'
--
/*!50003 DROP FUNCTION IF EXISTS `Get_Comment_Vote_Score` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`main`@`%` FUNCTION `Get_Comment_Vote_Score`(
    in_comment_id char(36)
) RETURNS bigint
    READS SQL DATA
BEGIN

    declare result bigint;
    
    select 
        v.score
    into 
        result
    from View_Comment_Vote_Totals v
    where
        v.comment_id = in_comment_id;
    
    
    if result is null then
    set result = 0;
    
    END IF;
    

	-- return the customer level
	RETURN (result);
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Current Database: `Deadit_Dev`
--

USE `Deadit_Dev`;

--
-- Final view structure for view `View_Api_Access_Token`
--

/*!50001 DROP VIEW IF EXISTS `View_Api_Access_Token`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`main`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `View_Api_Access_Token` AS select `t`.`id` AS `token_id`,`t`.`token` AS `token_token`,`t`.`user_id` AS `token_user_id`,`t`.`created_on` AS `token_created_on`,`u`.`user_id` AS `user_id`,`u`.`user_email` AS `user_email`,`u`.`user_username` AS `user_username`,`u`.`user_password` AS `user_password`,`u`.`user_created_on` AS `user_created_on` from (`Api_Access_Token` `t` join `View_User` `u` on((`u`.`user_id` = `t`.`user_id`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `View_Comment`
--

/*!50001 DROP VIEW IF EXISTS `View_Comment`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`main`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `View_Comment` AS select `c`.`id` AS `comment_id`,`c`.`author_id` AS `comment_author_id`,`c`.`post_id` AS `comment_post_id`,`c`.`content` AS `comment_content`,`c`.`parent_id` AS `comment_parent_id`,`c`.`created_on` AS `comment_created_on`,`c`.`deleted_on` AS `comment_deleted_on`,ifnull(`votes`.`count_upvotes`,0) AS `comment_count_votes_up`,ifnull(`votes`.`count_downvotes`,0) AS `comment_count_votes_down`,ifnull(`votes`.`count_novotes`,0) AS `comment_count_votes_none`,ifnull(`votes`.`score`,0) AS `comment_count_votes_score`,`u`.`username` AS `comment_author_username`,`p`.`community_id` AS `community_id`,`com`.`name` AS `community_name` from ((((`Comment` `c` join `User` `u` on((`u`.`id` = `c`.`author_id`))) left join `View_Comment_Vote_Totals` `votes` on((`votes`.`comment_id` = `c`.`id`))) join `Post` `p` on((`p`.`id` = `c`.`post_id`))) join `Community` `com` on((`com`.`id` = `p`.`community_id`))) group by `c`.`id` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `View_Comment_Vote_Totals`
--

/*!50001 DROP VIEW IF EXISTS `View_Comment_Vote_Totals`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`main`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `View_Comment_Vote_Totals` AS select `v`.`comment_id` AS `comment_id`,count((case when (`v`.`vote_type` = 1) then 1 end)) AS `count_novotes`,count((case when (`v`.`vote_type` = 2) then 1 end)) AS `count_downvotes`,count((case when (`v`.`vote_type` = 3) then 1 end)) AS `count_upvotes`,(count((case when (`v`.`vote_type` = 3) then 1 end)) - count((case when (`v`.`vote_type` = 2) then 1 end))) AS `score` from `Vote_Comment` `v` group by `v`.`comment_id` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `View_Community`
--

/*!50001 DROP VIEW IF EXISTS `View_Community`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`main`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `View_Community` AS select `c`.`id` AS `community_id`,`c`.`name` AS `community_name`,`c`.`title` AS `community_title`,`c`.`owner_id` AS `community_owner_id`,`c`.`description` AS `community_description`,`c`.`created_on` AS `community_created_on`,(select count(0) from `Community_Membership` `cm` where (`cm`.`community_id` = `c`.`id`)) AS `community_count_members` from `Community` `c` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `View_Community_Membership`
--

/*!50001 DROP VIEW IF EXISTS `View_Community_Membership`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`main`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `View_Community_Membership` AS with `community_membership_counts` as (select `counts`.`community_id` AS `community_id`,count(0) AS `count_members` from `Community_Membership` `counts` group by `counts`.`community_id`) select `cm`.`community_id` AS `community_id`,`c`.`name` AS `community_name`,`c`.`title` AS `community_title`,`c`.`owner_id` AS `community_owner_id`,`c`.`description` AS `community_description`,`c`.`created_on` AS `community_created_on`,`community_membership_counts`.`count_members` AS `community_count_members`,`cm`.`user_id` AS `user_id`,`u`.`user_email` AS `user_email`,`u`.`user_username` AS `user_username`,`u`.`user_password` AS `user_password`,`u`.`user_created_on` AS `user_created_on`,`cm`.`created_on` AS `user_joined_community_on` from (((`Community_Membership` `cm` left join `Community` `c` on((`c`.`id` = `cm`.`community_id`))) left join `View_User` `u` on((`u`.`user_id` = `cm`.`user_id`))) left join `community_membership_counts` on((`community_membership_counts`.`community_id` = `cm`.`community_id`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `View_Error_Message`
--

/*!50001 DROP VIEW IF EXISTS `View_Error_Message`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`main`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `View_Error_Message` AS select `e`.`id` AS `error_message_id`,`g`.`name` AS `error_message_group_name`,`e`.`message` AS `error_message_message`,`e`.`created_on` AS `error_message_created_on`,`e`.`error_message_group_id` AS `error_message_group_id`,`g`.`created_on` AS `error_message_group_created_on` from (`Error_Message` `e` join `Error_Message_Group` `g` on((`g`.`id` = `e`.`error_message_group_id`))) order by `e`.`id` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `View_Post`
--

/*!50001 DROP VIEW IF EXISTS `View_Post`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`main`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `View_Post` AS select `p`.`id` AS `post_id`,`p`.`community_id` AS `post_community_id`,`p`.`title` AS `post_title`,`p`.`post_type` AS `post_post_type`,`p`.`author_id` AS `post_author_id`,`p`.`created_on` AS `post_created_on`,`p`.`deleted_on` AS `post_deleted_on`,`p`.`archived_on` AS `post_archived_on`,`p`.`mod_removed_on` AS `post_mod_removed_on`,(select count(0) from `Comment` `c` where (`c`.`post_id` = `p`.`id`)) AS `post_count_comments`,`c`.`community_id` AS `community_id`,`c`.`community_name` AS `community_name`,`c`.`community_title` AS `community_title`,`c`.`community_owner_id` AS `community_owner_id`,`c`.`community_description` AS `community_description`,`c`.`community_created_on` AS `community_created_on`,`c`.`community_count_members` AS `community_count_members`,`a`.`user_id` AS `user_id`,`a`.`user_email` AS `user_email`,`a`.`user_username` AS `user_username`,`a`.`user_password` AS `user_password`,`a`.`user_created_on` AS `user_created_on`,ifnull(`vote_totals`.`count_upvotes`,0) AS `post_count_votes_upvotes`,ifnull(`vote_totals`.`count_downvotes`,0) AS `post_count_votes_downvotes`,ifnull(`vote_totals`.`count_novotes`,0) AS `post_count_votes_novotes`,ifnull(`vote_totals`.`score`,0) AS `post_count_votes_score` from (((`Post` `p` join `View_Community` `c` on((`c`.`community_id` = `p`.`community_id`))) join `View_User` `a` on((`a`.`user_id` = `p`.`author_id`))) left join `View_Post_Vote_Totals` `vote_totals` on((`vote_totals`.`post_id` = `p`.`id`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `View_Post_Link`
--

/*!50001 DROP VIEW IF EXISTS `View_Post_Link`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`main`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `View_Post_Link` AS select `p`.`url` AS `post_url`,`v`.`post_id` AS `post_id`,`v`.`post_community_id` AS `post_community_id`,`v`.`post_title` AS `post_title`,`v`.`post_post_type` AS `post_post_type`,`v`.`post_author_id` AS `post_author_id`,`v`.`post_created_on` AS `post_created_on`,`v`.`post_deleted_on` AS `post_deleted_on`,`v`.`post_archived_on` AS `post_archived_on`,`v`.`post_mod_removed_on` AS `post_mod_removed_on`,`v`.`post_count_comments` AS `post_count_comments`,`v`.`community_id` AS `community_id`,`v`.`community_name` AS `community_name`,`v`.`community_title` AS `community_title`,`v`.`community_owner_id` AS `community_owner_id`,`v`.`community_description` AS `community_description`,`v`.`community_created_on` AS `community_created_on`,`v`.`community_count_members` AS `community_count_members`,`v`.`user_id` AS `user_id`,`v`.`user_email` AS `user_email`,`v`.`user_username` AS `user_username`,`v`.`user_password` AS `user_password`,`v`.`user_created_on` AS `user_created_on`,`v`.`post_count_votes_upvotes` AS `post_count_votes_upvotes`,`v`.`post_count_votes_downvotes` AS `post_count_votes_downvotes`,`v`.`post_count_votes_novotes` AS `post_count_votes_novotes`,`v`.`post_count_votes_score` AS `post_count_votes_score` from (`Post_Link` `p` join `View_Post` `v` on((`v`.`post_id` = `p`.`id`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `View_Post_Text`
--

/*!50001 DROP VIEW IF EXISTS `View_Post_Text`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`main`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `View_Post_Text` AS select `p`.`content` AS `post_content`,`v`.`post_id` AS `post_id`,`v`.`post_community_id` AS `post_community_id`,`v`.`post_title` AS `post_title`,`v`.`post_post_type` AS `post_post_type`,`v`.`post_author_id` AS `post_author_id`,`v`.`post_created_on` AS `post_created_on`,`v`.`post_deleted_on` AS `post_deleted_on`,`v`.`post_archived_on` AS `post_archived_on`,`v`.`post_mod_removed_on` AS `post_mod_removed_on`,`v`.`post_count_comments` AS `post_count_comments`,`v`.`community_id` AS `community_id`,`v`.`community_name` AS `community_name`,`v`.`community_title` AS `community_title`,`v`.`community_owner_id` AS `community_owner_id`,`v`.`community_description` AS `community_description`,`v`.`community_created_on` AS `community_created_on`,`v`.`community_count_members` AS `community_count_members`,`v`.`user_id` AS `user_id`,`v`.`user_email` AS `user_email`,`v`.`user_username` AS `user_username`,`v`.`user_password` AS `user_password`,`v`.`user_created_on` AS `user_created_on`,`v`.`post_count_votes_upvotes` AS `post_count_votes_upvotes`,`v`.`post_count_votes_downvotes` AS `post_count_votes_downvotes`,`v`.`post_count_votes_novotes` AS `post_count_votes_novotes`,`v`.`post_count_votes_score` AS `post_count_votes_score` from (`Post_Text` `p` join `View_Post` `v` on((`v`.`post_id` = `p`.`id`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `View_Post_Vote_Totals`
--

/*!50001 DROP VIEW IF EXISTS `View_Post_Vote_Totals`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`main`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `View_Post_Vote_Totals` AS select `v`.`post_id` AS `post_id`,ifnull(count((case when (`v`.`vote_type` = 1) then 1 end)),0) AS `count_novotes`,ifnull(count((case when (`v`.`vote_type` = 2) then 1 end)),0) AS `count_downvotes`,ifnull(count((case when (`v`.`vote_type` = 3) then 1 end)),0) AS `count_upvotes`,(ifnull(count((case when (`v`.`vote_type` = 3) then 1 end)),0) - ifnull(count((case when (`v`.`vote_type` = 2) then 1 end)),0)) AS `score` from `Vote_Post` `v` group by `v`.`post_id` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `View_User`
--

/*!50001 DROP VIEW IF EXISTS `View_User`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`main`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `View_User` AS select `u`.`id` AS `user_id`,`u`.`email` AS `user_email`,`u`.`username` AS `user_username`,`u`.`password` AS `user_password`,`u`.`created_on` AS `user_created_on` from `User` `u` order by `u`.`id` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `View_Vote_Comment`
--

/*!50001 DROP VIEW IF EXISTS `View_Vote_Comment`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`main`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `View_Vote_Comment` AS select `v`.`comment_id` AS `vote_comment_comment_id`,`v`.`user_id` AS `vote_comment_user_id`,`v`.`vote_type` AS `vote_comment_vote_type`,`v`.`created_on` AS `vote_comment_created_on`,`u`.`username` AS `vote_comment_username`,`c`.`comment_post_id` AS `post_id`,`c`.`comment_count_votes_up` AS `comment_count_votes_up`,`c`.`comment_count_votes_down` AS `comment_count_votes_down`,`c`.`comment_count_votes_none` AS `comment_count_votes_none`,`c`.`comment_count_votes_score` AS `comment_count_votes_score` from ((`Vote_Comment` `v` join `User` `u` on((`u`.`id` = `v`.`user_id`))) left join `View_Comment` `c` on((`c`.`comment_id` = `v`.`comment_id`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `View_Vote_Post`
--

/*!50001 DROP VIEW IF EXISTS `View_Vote_Post`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`main`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `View_Vote_Post` AS select `vote`.`post_id` AS `vote_post_post_id`,`vote`.`user_id` AS `vote_post_user_id`,`vote`.`vote_type` AS `vote_post_vote_type`,`vote`.`created_on` AS `vote_post_created_on`,`u`.`username` AS `vote_post_user_username`,`p`.`post_community_id` AS `post_community_id`,`p`.`community_name` AS `post_community_name`,`p`.`post_post_type` AS `post_post_type`,`p`.`post_count_votes_upvotes` AS `post_count_votes_upvotes`,`p`.`post_count_votes_downvotes` AS `post_count_votes_downvotes`,`p`.`post_count_votes_novotes` AS `post_count_votes_novotes`,`p`.`post_count_votes_score` AS `post_count_votes_score` from ((`Vote_Post` `vote` join `User` `u` on((`u`.`id` = `vote`.`user_id`))) join `View_Post` `p` on((`p`.`post_id` = `vote`.`post_id`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-05-22 15:00:11
-- MySQL dump 10.13  Distrib 8.0.22, for Win64 (x86_64)
--
-- Host: 104.225.208.163    Database: Deadit_Dev
-- ------------------------------------------------------
-- Server version	8.0.36-0ubuntu0.20.04.1

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Dumping data for table `Vote_Type`
--
-- ORDER BY:  `id`

LOCK TABLES `Vote_Type` WRITE;
/*!40000 ALTER TABLE `Vote_Type` DISABLE KEYS */;
REPLACE INTO `Vote_Type` VALUES (1,'Novote'),(2,'Downvote'),(3,'Upvote');
/*!40000 ALTER TABLE `Vote_Type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Post_Type`
--
-- ORDER BY:  `id`

LOCK TABLES `Post_Type` WRITE;
/*!40000 ALTER TABLE `Post_Type` DISABLE KEYS */;
REPLACE INTO `Post_Type` VALUES (1,'Text'),(2,'Link');
/*!40000 ALTER TABLE `Post_Type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Error_Message`
--
-- ORDER BY:  `id`

LOCK TABLES `Error_Message` WRITE;
/*!40000 ALTER TABLE `Error_Message` DISABLE KEYS */;
REPLACE INTO `Error_Message` VALUES (200,2,'The email is already registered with another account.','2024-05-07 14:11:22'),(201,2,'The username is already taken.','2024-05-07 14:11:22'),(202,2,'The password does not meet the criteria.','2024-05-07 14:11:23'),(300,3,'The community name contains an invalid character.','2024-05-07 14:12:27'),(301,3,'The community name already exists.','2024-05-07 14:12:27'),(302,3,'The community name is banned.','2024-05-07 14:12:27'),(400,4,'Either \'commentId\' or \'postId\' must be provided.','2024-05-15 20:54:03'),(401,4,'Only \'commentId\' or \'postId\' can have a value, not both.','2024-05-15 20:54:03');
/*!40000 ALTER TABLE `Error_Message` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Banned_Community_Name`
--
-- ORDER BY:  `id`

LOCK TABLES `Banned_Community_Name` WRITE;
/*!40000 ALTER TABLE `Banned_Community_Name` DISABLE KEYS */;
REPLACE INTO `Banned_Community_Name` VALUES (1,'arse'),(2,'ass'),(3,'asshole'),(4,'homosexual'),(5,'homophobic'),(6,'racist'),(7,'gay'),(8,'lgbt'),(9,'jew'),(10,'jewish'),(11,'anti_semitic'),(12,'chink'),(13,'muslims'),(14,'muslim'),(15,'isis'),(16,'islamophobe'),(17,'homophobe'),(18,'bombing'),(19,'sexyhot'),(20,'bastard'),(21,'bitch'),(22,'fucker'),(23,'cunt'),(24,'damn'),(25,'fuck'),(26,'goddamn'),(27,'shit'),(28,'motherfucker'),(29,'nigga'),(30,'nigger'),(31,'prick'),(32,'shit_ass'),(33,'shitass'),(34,'son_of_a_bitch'),(35,'whore'),(36,'thot'),(37,'slut'),(38,'faggot'),(39,'dick'),(40,'pussy'),(41,'penis'),(42,'vagina'),(43,'negro'),(44,'coon'),(45,'bitched'),(46,'sexist'),(47,'freaking'),(48,'cock'),(49,'sucker'),(50,'lick'),(51,'licker'),(52,'rape'),(53,'molest'),(54,'anal'),(55,'buttrape'),(56,'coont'),(57,'cancer'),(58,'sex'),(59,'retard'),(60,'fuckface'),(61,'dumbass'),(62,'5h1t'),(63,'5hit'),(64,'a_s_s'),(65,'a2m'),(66,'a55'),(67,'adult'),(68,'amateur'),(69,'anal_impaler'),(70,'anal_leakage'),(71,'anilingus'),(72,'anus'),(73,'ar5e'),(74,'arrse'),(75,'arsehole'),(76,'ass_fuck'),(77,'asses'),(78,'assfucker'),(79,'ass_fucker'),(80,'assfukka'),(81,'assholes'),(82,'assmucus'),(83,'assmunch'),(84,'asswhole'),(85,'autoerotic'),(86,'b00bs'),(87,'b17ch'),(88,'b1tch'),(89,'ballbag'),(90,'ballsack'),(91,'bangbros'),(92,'bareback'),(93,'beastial'),(94,'beastiality'),(95,'beef_curtain'),(96,'bellend'),(97,'bestial'),(98,'bestiality'),(99,'biatch'),(100,'bimbos'),(101,'birdlock'),(102,'bitch_tit'),(103,'bitcher'),(104,'bitchers'),(105,'bitches'),(106,'bitchin'),(107,'bitching'),(108,'bloody'),(109,'blow_job'),(110,'blow_me'),(111,'blow_mud'),(112,'blowjob'),(113,'blowjobs'),(114,'blue_waffle'),(115,'blumpkin'),(116,'boiolas'),(117,'bollock'),(118,'bollok'),(119,'boner'),(120,'boob'),(121,'boobs'),(122,'booobs'),(123,'boooobs'),(124,'booooobs'),(125,'booooooobs'),(126,'breasts'),(127,'buceta'),(128,'bugger'),(129,'bum'),(130,'bunny_fucker'),(131,'bust_a_load'),(132,'busty'),(133,'butt'),(134,'butt_fuck'),(135,'butthole'),(136,'buttmuch'),(137,'buttplug'),(138,'c0ck'),(139,'c0cksucker'),(140,'carpet_muncher'),(141,'carpetmuncher'),(142,'cawk'),(143,'choade'),(144,'chota_bags'),(145,'cipa'),(146,'cl1t'),(147,'clit'),(148,'clit_licker'),(149,'clitoris'),(150,'clits'),(151,'clitty_litter'),(152,'clusterfuck'),(153,'cnut'),(154,'cock_pocket'),(155,'cock_snot'),(156,'cockface'),(157,'cockhead'),(158,'cockmunch'),(159,'cockmuncher'),(160,'cocks'),(161,'cocksuck'),(162,'cocksucked'),(163,'cocksucker'),(164,'cock_sucker'),(165,'cocksucking'),(166,'cocksucks'),(167,'cocksuka'),(168,'cocksukka'),(169,'cok'),(170,'cokmuncher'),(171,'coksucka'),(172,'cop_some_wood'),(173,'cornhole'),(174,'corp_whore'),(175,'cox'),(176,'cum'),(177,'cum_chugger'),(178,'cum_dumpster'),(179,'cum_freak'),(180,'cum_guzzler'),(181,'cumdump'),(182,'cummer'),(183,'cumming'),(184,'cums'),(185,'cumshot'),(186,'cunilingus'),(187,'cunillingus'),(188,'cunnilingus'),(189,'cunt_hair'),(190,'cuntbag'),(191,'cuntlick'),(192,'cuntlicker'),(193,'cuntlicking'),(194,'cunts'),(195,'cuntsicle'),(196,'cunt_struck'),(197,'cut_rope'),(198,'cyalis'),(199,'cyberfuc'),(200,'cyberfuck'),(201,'cyberfucked'),(202,'cyberfucker'),(203,'cyberfuckers'),(204,'cyberfucking'),(205,'d1ck'),(206,'dick_hole'),(207,'dick_shy'),(208,'dickhead'),(209,'dildo'),(210,'dildos'),(211,'dink'),(212,'dinks'),(213,'dirsa'),(214,'dirty_sanchez'),(215,'dlck'),(216,'dog_fucker'),(217,'doggie_style'),(218,'doggiestyle'),(219,'doggin'),(220,'dogging'),(221,'donkeyribber'),(222,'doosh'),(223,'duche'),(224,'dyke'),(225,'eat_a_dick'),(226,'eat_hair_pie'),(227,'ejaculate'),(228,'ejaculated'),(229,'ejaculates'),(230,'ejaculating'),(231,'ejaculatings'),(232,'ejaculation'),(233,'ejakulate'),(234,'erotic'),(235,'f_u_c_k'),(236,'f_u_c_k_e_r'),(237,'f4nny'),(238,'facial'),(239,'fag'),(240,'fagging'),(241,'faggitt'),(242,'faggs'),(243,'fagot'),(244,'fagots'),(245,'fags'),(246,'fanny'),(247,'fannyflaps'),(248,'fannyfucker'),(249,'fanyy'),(250,'fatass'),(251,'fcuk'),(252,'fcuker'),(253,'fcuking'),(254,'feck'),(255,'fecker'),(256,'felching'),(257,'fellate'),(258,'fellatio'),(259,'fingerfuck'),(260,'fingerfucked'),(261,'fingerfucker'),(262,'fingerfuckers'),(263,'fingerfucking'),(264,'fingerfucks'),(265,'fist_fuck'),(266,'fistfuck'),(267,'fistfucked'),(268,'fistfucker'),(269,'fistfuckers'),(270,'fistfucking'),(271,'fistfuckings'),(272,'fistfucks'),(273,'flange'),(274,'flog_the_log'),(275,'fook'),(276,'fooker'),(277,'fuck_hole'),(278,'fuck_puppet'),(279,'fuck_trophy'),(280,'fuck_yo_mama'),(281,'fucka'),(282,'fuck_ass'),(283,'fuck_bitch'),(284,'fucked'),(285,'fuckers'),(286,'fuckhead'),(287,'fuckheads'),(288,'fuckin'),(289,'fucking'),(290,'fuckings'),(291,'fuckingshitmotherfucker'),(292,'fuckme'),(293,'fuckmeat'),(294,'fucks'),(295,'fucktoy'),(296,'fuckwhit'),(297,'fuckwit'),(298,'fudge_packer'),(299,'fudgepacker'),(300,'fuk'),(301,'fuker'),(302,'fukker'),(303,'fukkin'),(304,'fuks'),(305,'fukwhit'),(306,'fukwit'),(307,'fux'),(308,'fux0r'),(309,'gangbang'),(310,'gang_bang'),(311,'gangbanged'),(312,'gangbangs'),(313,'gassy_ass'),(314,'gaylord'),(315,'gaysex'),(316,'goatse'),(317,'god'),(318,'god_damn'),(319,'god_dam'),(320,'goddamned'),(321,'god_damned'),(322,'ham_flap'),(323,'hardcoresex'),(324,'hell'),(325,'heshe'),(326,'hoar'),(327,'hoare'),(328,'hoer'),(329,'homo'),(330,'homoerotic'),(331,'hore'),(332,'horniest'),(333,'horny'),(334,'hotsex'),(335,'how_to_kill'),(336,'how_to_murdep'),(337,'jackoff'),(338,'jack_off'),(339,'jap'),(340,'jerk'),(341,'jerk_off'),(342,'jism'),(343,'jiz'),(344,'jizm'),(345,'jizz'),(346,'kawk'),(347,'kinky_jesus'),(348,'knob'),(349,'knob_end'),(350,'knobead'),(351,'knobed'),(352,'knobend'),(353,'knobhead'),(354,'knobjocky'),(355,'knobjokey'),(356,'kock'),(357,'kondum'),(358,'kondums'),(359,'kum'),(360,'kummer'),(361,'kumming'),(362,'kums'),(363,'kunilingus'),(364,'kwif'),(365,'l3itch'),(366,'labia'),(367,'len'),(368,'lmao'),(369,'lmfao'),(370,'lust'),(371,'lusting'),(372,'m0f0'),(373,'m0fo'),(374,'m45terbate'),(375,'ma5terb8'),(376,'ma5terbate'),(377,'mafugly'),(378,'masochist'),(379,'masterb8'),(380,'masterbat3'),(381,'masterbate'),(382,'master_bate'),(383,'masterbation'),(384,'masterbations'),(385,'masturbate'),(386,'mof0'),(387,'mofo'),(388,'mo_fo'),(389,'mothafuck'),(390,'mothafucka'),(391,'mothafuckas'),(392,'mothafuckaz'),(393,'mothafucked'),(394,'mothafucker'),(395,'mothafuckers'),(396,'mothafuckin'),(397,'mothafucking'),(398,'mothafuckings'),(399,'mothafucks'),(400,'mother_fucker'),(401,'motherfuck'),(402,'motherfucked'),(403,'motherfuckers'),(404,'motherfuckin'),(405,'motherfucking'),(406,'motherfuckings'),(407,'motherfuckka'),(408,'motherfucks'),(409,'muff'),(410,'muff_puff'),(411,'mutha'),(412,'muthafecker'),(413,'muthafuckker'),(414,'muther'),(415,'mutherfucker'),(416,'n1gga'),(417,'n1gger'),(418,'nazi'),(419,'need_the_dick'),(420,'nigg3r'),(421,'nigg4h'),(422,'niggah'),(423,'niggas'),(424,'niggaz'),(425,'niggers'),(426,'nob'),(427,'nob_jokey'),(428,'nobhead'),(429,'nobjocky'),(430,'nobjokey'),(431,'numbnuts'),(432,'nut_butter'),(433,'nutsack'),(434,'omg'),(435,'orgasim'),(436,'orgasims'),(437,'orgasm'),(438,'orgasms'),(439,'p0rn'),(440,'pawn'),(441,'pecker'),(442,'penisfucker'),(443,'phonesex'),(444,'phuck'),(445,'phuk'),(446,'phuked'),(447,'phuking'),(448,'phukked'),(449,'phukking'),(450,'phuks'),(451,'phuq'),(452,'pigfucker'),(453,'pimpis'),(454,'piss'),(455,'pissed'),(456,'pisser'),(457,'pissers'),(458,'pisses'),(459,'pissflaps'),(460,'pissin'),(461,'pissing'),(462,'pissoff'),(463,'poop'),(464,'porn'),(465,'porno'),(466,'pornography'),(467,'pornos'),(468,'pricks'),(469,'pron'),(470,'pube'),(471,'pusse'),(472,'pussi'),(473,'pussies'),(474,'pussy_fart'),(475,'pussy_palace'),(476,'pussys'),(477,'queaf'),(478,'queer'),(479,'rectum'),(480,'rimjaw'),(481,'rimming'),(482,'s_hit'),(483,'s_h_i_t'),(484,'sadism'),(485,'sadist'),(486,'sandbar'),(487,'sausage_queen'),(488,'schlong'),(489,'screwing'),(490,'scroat'),(491,'scrote'),(492,'scrotum'),(493,'semen'),(494,'sh1t'),(495,'shag'),(496,'shagger'),(497,'shaggin'),(498,'shagging'),(499,'shemale'),(500,'shit_fucker'),(501,'shitdick'),(502,'shite'),(503,'shited'),(504,'shitey'),(505,'shitfuck'),(506,'shitfull'),(507,'shithead'),(508,'shiting'),(509,'shitings'),(510,'shits'),(511,'shitted'),(512,'shitter'),(513,'shitters'),(514,'shitting'),(515,'shittings'),(516,'shitty'),(517,'skank'),(518,'slope'),(519,'slut_bucket'),(520,'sluts'),(521,'smegma'),(522,'smut'),(523,'snatch'),(524,'spac'),(525,'spunk'),(526,'t1tt1e5'),(527,'t1tties'),(528,'teets'),(529,'teez'),(530,'testical'),(531,'testicle'),(532,'tit'),(533,'tit_wank'),(534,'titfuck'),(535,'tits'),(536,'titt'),(537,'tittie5'),(538,'tittiefucker'),(539,'titties'),(540,'tittyfuck'),(541,'tittywank'),(542,'titwank'),(543,'tosser'),(544,'turd'),(545,'tw4t'),(546,'twat'),(547,'twathead'),(548,'twatty'),(549,'twunt'),(550,'twunter'),(551,'v14gra'),(552,'v1gra'),(553,'viagra'),(554,'vulva'),(555,'w00se'),(556,'wang'),(557,'wank'),(558,'wanker'),(559,'wanky'),(560,'whoar'),(561,'willies'),(562,'willy'),(563,'wtf'),(564,'xrated'),(565,'xxx'),(566,'kys'),(567,'kill'),(568,'die'),(569,'cliff'),(570,'bridge'),(571,'shooting'),(572,'shoot'),(573,'bomb'),(574,'terrorist'),(575,'terrorism'),(576,'bombed'),(577,'trump'),(578,'maga'),(579,'conservative'),(580,'make_america_great_again'),(581,'far_right'),(582,'necrophilia'),(583,'mongoloid'),(584,'furfag'),(585,'cp'),(586,'pedo'),(587,'pedophile'),(588,'pedophilia'),(589,'child_predator'),(590,'predatory'),(591,'depression'),(592,'cut_myself'),(593,'i_want_to_die'),(594,'fuck_life'),(595,'redtube'),(596,'loli'),(597,'lolicon'),(598,'cub'),(637,'create'),(638,'deadit');
/*!40000 ALTER TABLE `Banned_Community_Name` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Error_Message_Group`
--
-- ORDER BY:  `id`

LOCK TABLES `Error_Message_Group` WRITE;
/*!40000 ALTER TABLE `Error_Message_Group` DISABLE KEYS */;
REPLACE INTO `Error_Message_Group` VALUES (1,'Misc','2024-05-07 14:07:53'),(2,'Account Signup','2024-05-07 14:08:56'),(3,'Create Community','2024-05-07 14:09:15'),(4,'Voting','2024-05-15 20:19:17');
/*!40000 ALTER TABLE `Error_Message_Group` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-05-22 15:00:16
