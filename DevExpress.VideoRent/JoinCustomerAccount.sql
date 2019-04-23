select N0.`Oid`,
	   N2.`OptimisticLockField`,
       N2.`ObjectType`,
       N1.`CreatedBy`,
       N1.`CreatedAt`,
       N1.`Tag`,
       N0.`Name`,
       N0.`Debit`,
       N0.`Credit`,
       N0.`Type`,
       N0.`AccountId`,
       N0.`Balance`,
       N0.`Customer` 
       from ((`Account` N0
			   inner join `VideoRentBaseObject` N1 on (N0.`Oid` = N1.`Oid`))
				inner join `ExtendedXPBaseObject` N2 on (N1.`Oid` = N2.`Oid`))
where (N0.`Customer` = '{3803df3e-5916-45c1-a15e-d1538dea3139}') 