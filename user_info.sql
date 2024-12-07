-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 07, 2024 at 09:04 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `user_info`
--

-- --------------------------------------------------------

--
-- Table structure for table `attendance_db`
--

CREATE TABLE `attendance_db` (
  `studentid` varchar(100) NOT NULL,
  `name` varchar(100) NOT NULL,
  `subject` varchar(100) NOT NULL,
  `course` varchar(100) NOT NULL,
  `year` varchar(100) NOT NULL,
  `section` varchar(100) NOT NULL,
  `date` date NOT NULL,
  `timein` time NOT NULL,
  `photo` longblob NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `login_acc`
--

CREATE TABLE `login_acc` (
  `username` varchar(100) NOT NULL,
  `password` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `login_acc`
--

INSERT INTO `login_acc` (`username`, `password`) VALUES
('admin', 'admin');

-- --------------------------------------------------------

--
-- Table structure for table `student_db`
--

CREATE TABLE `student_db` (
  `studentid` int(100) NOT NULL,
  `fname` varchar(100) NOT NULL,
  `gender` varchar(100) NOT NULL,
  `birthdate` date NOT NULL,
  `religion` varchar(100) NOT NULL,
  `citizen` varchar(100) NOT NULL,
  `faddress` varchar(100) NOT NULL,
  `contact` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `course` varchar(50) NOT NULL,
  `year` varchar(50) NOT NULL,
  `section` varchar(50) NOT NULL,
  `photo` longblob NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `teacherattendance_db`
--

CREATE TABLE `teacherattendance_db` (
  `id_no` varchar(100) NOT NULL,
  `name` varchar(100) NOT NULL,
  `department` int(100) NOT NULL,
  `email` int(100) NOT NULL,
  `timein` time NOT NULL,
  `date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `teacher_db`
--

CREATE TABLE `teacher_db` (
  `id_no` int(100) NOT NULL,
  `name` varchar(100) NOT NULL,
  `gender` varchar(50) NOT NULL,
  `department` varchar(50) NOT NULL,
  `email` varchar(50) NOT NULL,
  `contact` varchar(50) NOT NULL,
  `photo` longblob NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
