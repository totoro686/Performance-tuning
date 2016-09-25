--
-- for the given table
-- list the indexes on the table
--
-- parameter 1 = owner
-- parameter 2 = table_name
--
-- usage is: @SHOWINDEXES <owner> <table_name>
--

set verify off

break on index_name skip 1
col column_name format a30

select index_name,column_name
,(select index_type from dba_indexes b where b.owner = a.index_owner and b.index_name = a.index_name) index_type
,(select uniqueness from dba_indexes b where b.owner = a.index_owner and b.index_name = a.index_name) uniqueness
,(select tablespace_name from dba_indexes b where b.owner = a.index_owner and b.index_name = a.index_name) tablespace_name
from dba_ind_columns a
where table_name = upper('&&2')
and table_owner = upper('&&1')
order by 1,column_position
/
