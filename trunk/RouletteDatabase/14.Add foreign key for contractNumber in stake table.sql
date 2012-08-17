Alter table Stake
add foreign key(ContractNumber) references [Check](ContractNumber) ON DELETE CASCADE;