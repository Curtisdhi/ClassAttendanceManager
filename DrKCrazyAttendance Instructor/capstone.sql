-- phpMyAdmin SQL Dump
-- version 3.5.8.2
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Jan 26, 2015 at 07:37 PM
-- Server version: 5.1.73
-- PHP Version: 5.5.13

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `capstone`
--

-- --------------------------------------------------------

--
-- Table structure for table `Attendance`
--

CREATE TABLE IF NOT EXISTS `Attendance` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `studentId` int(11) NOT NULL,
  `courseId` int(11) NOT NULL,
  `timeLog` datetime NOT NULL,
  `tardy` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `studentId` (`studentId`),
  KEY `courseId` (`courseId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `Courses`
--

CREATE TABLE IF NOT EXISTS `Courses` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `classroom` varchar(10) NOT NULL,
  `name` varchar(10) NOT NULL,
  `section` varchar(5) NOT NULL,
  `semester` varchar(6) NOT NULL,
  `days` varchar(6) NOT NULL,
  `startDate` date NOT NULL,
  `endDate` date NOT NULL,
  `startTime` time NOT NULL,
  `endTime` time NOT NULL,
  `logTardy` tinyint(1) NOT NULL,
  `gracePeriod` time NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Dumping data for table `Courses`
--

INSERT INTO `Courses` (`id`, `classroom`, `name`, `section`, `semester`, `days`, `startDate`, `endDate`, `startTime`, `endTime`, `logTardy`, `gracePeriod`) VALUES
(1, 'CISP 2010', 'test', '45', 'Fall', 'MWF', '2015-01-01', '2015-01-04', '01:00:00', '02:00:00', 1, '01:00:00'),
(2, 'CISP 2010', 'CISP 2010', 'A01', 'Spring', 'MW', '2015-01-01', '2015-05-15', '13:00:00', '16:00:00', 0, '00:00:19');

-- --------------------------------------------------------

--
-- Table structure for table `Students`
--

CREATE TABLE IF NOT EXISTS `Students` (
  `id` int(11) NOT NULL,
  `username` varchar(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `Attendance`
--
ALTER TABLE `Attendance`
  ADD CONSTRAINT `Attendance_ibfk_2` FOREIGN KEY (`courseId`) REFERENCES `Courses` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `Attendance_ibfk_1` FOREIGN KEY (`studentId`) REFERENCES `Students` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
