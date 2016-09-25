--
-- show a query diagram for the current plan of the plan_table
-- uses the join tree with extended information
--
-- usage is: @SHOWPLANQUERYDIAGRAM11G
--

col query_diagram format a300
col query_name format a30
break on query_name skip 1 dup

with
     plan_table_current as (
                             select *
                             from plan_table
                             where plan_id = (select max(plan_id) from plan_table)
                           )
   , predicate_data as (
                         select nvl(query_name,'SEL$1') adjusted_query_name
                               ,z.*
                         from (
                               select case
                                            when table_2_alias is null then 'CONSTANT TEST'
                                            else 'JOIN'
                                       end expression_type
                                      ,substr(pt.object_alias,instr(object_alias,'@')+1) query_name
                                      ,y.*
                                from plan_table_current pt 
                                    ,(
                                       select case
                                                   when instr(predicate_expression,'.') > 0 then substr(predicate_expression,1,instr(predicate_expression,'.')-1)
                                                   else (select substr(object_alias,1,instr(object_alias,'@')-1) from plan_table_current where id = x.id and rownum = 1)
                                              end table_1_alias
                                             ,substr(predicate_expression,instr(predicate_expression,'=')+1,instr(predicate_expression,'.',1,2)-instr(predicate_expression,'=')-1) table_2_alias
                                             ,x.*
                                       from (
                                              select distinct *
                                              from (
                                                     select id,substr(      ' '||p||' '
                                                                     ,instr(' '||p||' ',' ',1,level)+1
                                                                     ,instr(' '||p||' ',' ',1,level+1)-instr(' '||p||' ',' ',1,level)-1
                                                                     ) predicate_expression
                                                            ,level rowno
                                                            ,p
                                                     from (
                                                            select *
                                                            from (
                                                                   select id,trim(
                                                                          replace(
                                                                          replace(
                                                                          replace(
                                                                          replace(
                                                                          replace(
                                                                          replace(
                                                                          replace(
                                                                          replace(
                                                                                  p
                                                                                 ,'         ',' ')
                                                                                 ,'        ',' ')
                                                                                 ,'       ',' ')
                                                                                 ,'      ',' ')
                                                                                 ,'     ',' ')
                                                                                 ,'    ',' ')
                                                                                 ,'   ',' ')
                                                                                 ,'  ',' ')
                                                                         ) p
                                                                   from (
                                                                          select id,trim(
                                                                                 replace(
                                                                                 replace(
                                                                                 replace(
                                                                                 replace(
                                                                                 replace(
                                                                                 replace(
                                                                                 replace(
                                                                                 replace(
                                                                                         p
                                                                                        ,'"')
                                                                                        ,' AND ',' ')
                                                                                        ,' OR ',' ')
                                                                                        ,' NOT ',' ')
                                                                                        ,'(',' ')
                                                                                        ,')',' ')
                                                                                        ,' and ',' ')
                                                                                        ,' and ',' ')
                                                                                ) p
                                                                          from (
                                                                                 select id,access_predicates||' '||filter_predicates p
                                                                                 from plan_table_current
                                                                                 where rownum >= 1
                                                                               )
                                                                        )
                                                                 )
                                                            where p is not null
                                                          )
                                                     connect by level <= length(p)-length(replace(p,' '))+1
                                                   )
                                           ) x
                                     ) y
                               where y.id = pt.id
                              ) z
                       )
   , constant_test as (
                         select distinct expression_type,table_1_alias
                         from predicate_data
                         where expression_type = 'CONSTANT TEST'
                      )
select t.query_name
      ,decode(ct,'c','c',' ')||lpad(decode(ct,'c','-',' '),(lvlno)*3,decode(ct,'c','-',' '))||table_2_alias query_diagram
from (
       select query_name,table_1_alias,table_2_alias
            ,level lvlno
            ,max(level) over (partition by query_name) max_lvlno
            ,case when (select expression_type from constant_test where table_1_alias = x.table_2_alias) is not null then 'c' end ct
       from (
              select distinct adjusted_query_name query_name,table_1_alias,table_2_alias
              from predicate_data
              where expression_type = 'JOIN'
              union all
              select distinct adjusted_query_name query_name,null,table_1_alias
              from predicate_data
              where table_1_alias not in (select table_2_alias from predicate_data where table_2_alias is not null)
            ) x
       connect by prior table_2_alias = table_1_alias
       and prior query_name = query_name
       start with table_1_alias is null
     ) t
/
