--
-- show actual unique list of tables referenced by the QEP
--

with
      plan_table_current as (
                              select *
                              from plan_table
                              where plan_id = (select max(plan_id) from plan_table)
                            )
select object_owner,object_name,replace(object_alias,'@','   @   ') object_alias
from plan_table_current
where object_type = 'TABLE'
union
select b.table_owner,b.table_name,replace(object_alias,'@','   @   ') object_alias
from plan_table_current a
    ,dba_indexes b
where a.object_type like 'INDEX%'
and a.object_owner = b.owner
and a.object_name = b.index_name
order by 1,2
/
