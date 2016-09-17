--
-- dumps a crude ascii art data model for the current plan in the plan_table
--
-- usage is: @SHOWPLANDATAMODEL11G
--

with
       table_list as (
                        select b.*,chr(rownum+96) dm_key
                        from (
                               select a.*
                               from (
                                      select object_owner owner,object_name table_name
                                      from plan_table
                                      where object_type = 'TABLE'
                                      and plan_id = (select max(plan_id) from plan_table)
                                      union
                                      select b.table_owner,b.table_name
                                      from plan_table a
                                          ,dba_indexes b
                                      where a.object_type like 'INDEX%'
                                      and a.object_owner = b.owner
                                      and a.object_name = b.index_name
                                      and a.plan_id = (select max(plan_id) from plan_table)
                                    ) a
                               order by a.table_name,a.owner
                            ) b
                     )
     , constraint_data as (
                            select a.owner,a.table_name,a.constraint_type,a.constraint_name,a.r_owner,a.r_constraint_name
                                  ,nvl(b.owner,a.owner) parent_owner,nvl(b.table_name,a.table_name) parent_table_name
                                  ,decode(a.constraint_type,'R',a.owner) child_owner,decode(a.constraint_type,'R',a.table_name) child_table_name
                            from dba_constraints a
                                ,dba_constraints b
                            where a.constraint_type in ('P','R')
                            and (a.owner,a.table_name) in (select owner,table_name from table_list)
                            and a.r_owner = b.owner(+)
                            and a.r_constraint_name = b.constraint_name(+)
                            union all
                            select owner,table_name,'P',null,null,null,null,null,null,null
                            from table_list a
                            where not exists (
                                               select null
                                               from dba_constraints b
                                               where b.owner = a.owner
                                               and b.table_name = a.table_name
                                               and b.constraint_type = 'P'
                                             )
                           )
    , table_list_plus as (
                                                       select a.owner,a.table_name,listagg(c.dm_key,',') within group (order by c.dm_key) parent_dm_key_list
                                                       from table_list a
                                                           ,constraint_data b
                                                           ,table_list c
                                                       where a.owner = b.child_owner
                                                       and a.table_name = b.child_table_name
                                                       and b.parent_owner = c.owner
                                                       and b.parent_table_name = c.table_name
                                                       group by a.owner,a.table_name
                        )
    , table_level as (
                       select c.*,(ordinal_position)*prefix_spaces+(ordinal_position-1)*3+cumm_table_name_length-length(table_name)+1 start_position
                       from (
                              select b.*,trunc((max_total_table_name_length-total_table_name_length)/(table_count+1)) prefix_spaces
                              from (
                                     select a.*,max(table_count) over () max_table_count,max(total_table_name_length) over() max_total_table_name_length
                                     from (
                                            select owner,table_name,max(lvlno) dm_lvl,max_dm_lvl
                                                  ,sum(length(table_name)) over (partition by max(lvlno)) total_table_name_length
                                                  ,count(*) over(partition by max(lvlno)) table_count
                                                  ,row_number() over(partition by max(lvlno) order by table_name,owner) ordinal_position
                                                  ,sum(length(table_name)) over (partition by max(lvlno) order by table_name,owner) cumm_table_name_length
                                            from (
                                                   select level lvlno,max(level) over () max_dm_lvl,a.*
                                                   from constraint_data a
                                                   connect by r_owner = prior owner
                                                   and r_constraint_name = prior constraint_name
                                                   start with r_owner is null
                                                 )
                                            group by owner,table_name,max_dm_lvl
                                          ) a
                                   ) b
                            ) c
                     )
    , table_lines as (
                       select a.*
                       from (
                              select table_level.*,replace(substr(sys_connect_by_path(lpad(' ',prefix_spaces,' ')||table_name,','),2),',','   ') line_text
                              from table_level
                              connect by prior dm_lvl = dm_lvl
                              and prior ordinal_position = ordinal_position - 1
                              start with ordinal_position = 1
                            ) a
                       where ordinal_position = table_count
                       order by dm_lvl
                     )
    , print_array as (
                       select x.*
                             ,case
                                  when exists (
                                               select null
                                               from table_level b2
                                                   ,constraint_data c2
                                                   ,table_level d2
                                               where x.columnno = b2.start_position
                                               and b2.owner = c2.parent_owner
                                               and b2.table_name = c2.parent_table_name
                                               and c2.child_owner = d2.owner
                                               and c2.child_table_name = d2.table_name
                                               and (
                                                     x.dm_lvl < d2.dm_lvl-1 or
                                                     x.dm_lvl = d2.dm_lvl-1 and x.rowno in (1,2)
                                                   )
                                             ) then (
                                                      select e2.dm_key
                                                      from table_level b2
                                                          ,constraint_data c2
                                                          ,table_level d2
                                                          ,table_list e2
                                                      where x.columnno = b2.start_position
                                                      and b2.owner = c2.parent_owner
                                                      and b2.table_name = c2.parent_table_name
                                                      and c2.child_owner = d2.owner
                                                      and c2.child_table_name = d2.table_name
                                                      and (
                                                            x.dm_lvl < d2.dm_lvl-1 or
                                                            x.dm_lvl = d2.dm_lvl-1 and x.rowno in (1,2)
                                                          )
                                                      and c2.parent_owner = e2.owner
                                                      and c2.parent_table_name = e2.table_name
                                                    )
                                 when exists (
                                               select null
                                               from table_level b2
                                                   ,constraint_data c2
                                                   ,table_level d2
                                               where x.dm_lvl = b2.dm_lvl-1
                                               and x.rowno in (3)
                                               and b2.owner = c2.child_owner
                                               and b2.table_name = c2.child_table_name
                                               and c2.parent_owner = d2.owner
                                               and c2.parent_table_name = d2.table_name
                                               and (x.columnno between b2.start_position and d2.start_position or
                                                    x.columnno between d2.start_position and b2.start_position)
                                             ) then '-'
                                 when exists (
                                               select null
                                               from table_level b2
                                                   ,constraint_data c2
                                               where x.columnno = b2.start_position
                                               and b2.owner = c2.child_owner
                                               and b2.table_name = c2.child_table_name
                                               and x.dm_lvl = b2.dm_lvl-1
                                               and x.rowno in (4,5)
                                             ) then (
                                                      select decode(x.rowno,4,'|','| ('||f2.parent_dm_key_list||')')
                                                      from table_level b2
                                                          ,constraint_data c2
                                                          ,table_list_plus f2
                                                      where x.columnno = b2.start_position
                                                      and b2.owner = c2.child_owner
                                                      and b2.table_name = c2.child_table_name
                                                      and x.dm_lvl = b2.dm_lvl-1
                                                      and x.rowno in (4,5)
                                                      and b2.owner = f2.owner
                                                      and b2.table_name = f2.table_name
                                                    )
                                 else ' '
                              end cell_value
                       from (
                              select c.dm_lvl,2 rowtype,b.rowno,a.rowno columnno
                              from (
                                     select rownum rowno
                                     from dual
                                     connect by level <= (select max(length(line_text)) from table_lines)
                                   ) a
                                  ,(
                                     select level rowno
                                     from dual
                                     connect by level <= 5
                                   ) b
                                 ,table_lines c
                              where c.dm_lvl < c.max_dm_lvl
                              and a.rowno <= length(c.line_text)
                           ) x
                     )
    , constraint_lines as (
                            select *
                            from (
                                   select dm_lvl,rowtype,rowno,columnno,connect_by_isleaf cbil,replace(sys_connect_by_path(cell_value,','),',') line_text
                                   from print_array
                                   connect by prior dm_lvl = dm_lvl
                                   and prior rowtype = rowtype
                                   and prior rowno = rowno
                                   and prior columnno = columnno-1
                                   start with columnno = 1
                                 )
                            where cbil = 1
                          )
     , data_model as (
                       select dm_lvl,1 rowtype,1 rowno,line_text
                       from table_lines
                       union all
                       select dm_lvl,rowtype,rowno,line_text
                       from constraint_lines
                       order by 1,2,3
                     )
select '     '||line_text data_model from data_model 
/
