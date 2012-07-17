Alter table Game
Add CashierId int not null;
Alter table Game
Add foreign key (Id) REFERENCES Cashier(Id);