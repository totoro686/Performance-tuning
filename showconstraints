--
-- given a table
-- show is PK/UK/FK constraints
-- and FK constraints that point to it
--
-- usage is: @SHOWCONSTRAINTS <owner> <table_name>
--

break on constraint_name skip 1 on parent_table_name
col column_name format a30
col sort_order noprint
col select_id noprint

select a.owner,a.table_name,a.constraint_name,a.constraint_type,c.column_name,b.owner parent_child_owner,b.table_name parent_child_table_name,b.constraint_type,a.index_name,decode(a.constraint_type,'P',1,'U',2) sort_order,1 select_id
from dba_constraints a
    ,dba_constraints b
    ,dba_cons_columns c
where a.owner = upper('&&1')
and a.table_name = upper('&&2')
and a.r_owner = b.owner(+)
and a.r_constraint_name = b.constraint_name(+)
and a.owner = c.owner
and a.constraint_name = c.constraint_name
and a.constraint_type != 'C'
union all
select a.owner,a.table_name,a.constraint_name,a.constraint_type,c.column_name,b.owner parent_child_owner,b.table_name parent_child_table_name,b.constraint_type,a.index_name,decode(a.constraint_type,'P',1,'U',2) sort_order,2 select_id
from dba_constraints a
    ,dba_constraints b
    ,dba_cons_columns c
where b.owner = upper('&&1')
and b.table_name = upper('&&2')
and b.constraint_type in ('P','U')
and a.r_owner = b.owner
and a.r_constraint_name = b.constraint_name
and b.table_name != a.table_name
and a.owner = c.owner
and a.constraint_name = c.constraint_name
order by select_id,sort_order,owner,table_name,constraint_name,column_name
/
