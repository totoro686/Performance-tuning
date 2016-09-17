--
-- construct count queries for tables referenced by the most recent plan in the plan_table
--
-- usage is: @SHOWPLANCOUNTQUERIES11G
--

select count_query_sqltext||decode(lead(count_query_sqltext) over (order by object_owner,object_name),null,';',' union all') sql_text
from (
       select distinct object_owner,object_name,'select count(*) rowcount,'''||object_owner||''' owner,'''||object_name||''' table_name from '||object_owner||'.'||object_name count_query_sqltext
       from (
              select id,object_owner,object_name,replace(object_alias,'@','   @   ') object_alias
              from plan_table
              where object_type = 'TABLE'
              and plan_id = (select max(plan_id) from plan_table)
              union
              select id,b.table_owner,b.table_name,replace(object_alias,'@','   @   ') object_alias
              from plan_table a
                  ,dba_indexes b
              where a.object_type like 'INDEX%'
              and a.object_owner = b.owner
              and a.object_name = b.index_name
              and a.plan_id = (select max(plan_id) from plan_table)
            )
       order by object_owner,object_name
     )
/
