Alter table Game
Add CashierId int not null;
Alter table Game
Add foreign key (CashierId) REFERENCES Cashier(Id);