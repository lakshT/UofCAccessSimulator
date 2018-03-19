create database acSimulator
use acSimulator

create table userpath(uid int primary key, uname nvarchar(20), upath nvarchar(500))
create table usertime(uid int primary key, utime nvarchar(500))


insert into userpath values(1003,'User3','AS10:AS9:AS8:AS12:AS7:AS11:AS5')
update usertime set utime = '5:51PM;5:51PM;5:52PM;5:53PM;5:55PM' where uid = 1001
update usertime set utime = '5:51PM;5:51PM;5:51PM;5:52PM;5:53PM;5:54PM' where uid = 1002
update usertime set utime = '5:51PM;5:51PM;5:52PM;5:53PM;5:54PM;5:55PM;5:56PM' where uid = 1003

select * from userpath
select * from usertime

select utime from usertime as t,userpath as p where p.uid = t.uid and p.uname = 'User1'