CREATE TABLE [GameStates]
(
	Id int identity(1,1) primary key,
	[State] smallint NOT NULL,
	StartTime datetime NOT NULL,
);
Go