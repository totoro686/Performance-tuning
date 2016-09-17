--
-- dump the most recent plan in the plan_table
-- but do not include lesser used sections of the plan
--
-- usage is: @SHOWPLAN11GSHORT
--

select * from table(dbms_xplan.display('PLAN_TABLE',NULL,'ADVANCED -projection -outline -alias'));
