--clear breaks
--clear computes
--clear columns
col time_remaining head 'Seconds|Remaining'
col scanned_blocks head 'Scanned Blocks|or Indexes'
col all_blocks head 'All Blocks|or Indexes'
col blocks_remaining head 'Blocks|or Indexes|Remaining' noprint
col opname format a16
col target format a40
col username format a15
col MB_per_Second form 990.0 head 'MB/s'
col pct_scanned head '%Scanned' format 990.00
col predicted_runtime_seconds head 'Estmd.|Runtime|Seconds'
col total_blocks head 'Total|Blocks|or|Indexes'
col sid format 99990
col block_size format 99990 head 'Block|Size'
col id_passes_temp format a25
break on inst_id skip page

with
      scan_data as (
                     select
                              to_number(
                                     substr(a.message
                                    ,instr(a.message,': ',1,2)+2
                                    ,instr(a.message,' out of ',1,1)-instr(a.message,': ',1,2)-1
                                    )
                                       )
                            / to_number(
                                     substr(a.message
                                    ,instr(a.message,' out of ',1,1)+8
                                    ,instr(a.message,' ',instr(a.message,' out of ',1,1)+8)-instr(a.message,' out of ',1,1)-7
                                    )
                                       ) *100 pct_scanned
                           ,  to_number(
                                     substr(a.message
                                    ,instr(a.message,' out of ',1,1)+8
                                    ,instr(a.message,' ',instr(a.message,' out of ',1,1)+8)-instr(a.message,' out of ',1,1)-7
                                    )
                                       )
                            - to_number(
                                     substr(a.message
                                    ,instr(a.message,': ',1,2)+2
                                    ,instr(a.message,' out of ',1,1)-instr(a.message,': ',1,2)-1
                                    )
                                       ) blocks_remaining
                            , a.time_remaining
                            , a.opname
                            , to_number(b.value) block_size
                            , a.target
                            , a.sid
                            , a.inst_id
                            , a.username
                            , a.sql_hash_value
                     from   (
                              select
                                      replace(gv$session_longops.message,'RMAN:','RMAN') message
                                    , gv$session_longops.time_remaining
                                    , gv$session_longops.opname
                                    , nvl(gv$session_longops.target,replace(gv$session_longops.target_desc,'Table ')) target
                                    , gv$session_longops.sid
                                    , gv$session_longops.inst_id
                                    , gv$session_longops.username
                                    , gv$session_longops.sql_hash_value
                              from  gv$session_longops
                                  , gv$session
                              where gv$session_longops.sid = gv$session.sid
                              and gv$session_longops.inst_id = gv$session.inst_id
                              and gv$session_longops.sid = gv$session.sid
                              and gv$session_longops.serial# = gv$session.serial#
                              and gv$session_longops.time_remaining > 0
and gv$session_longops.username = user
                            ) a
                           ,(select value from gv$parameter where name = 'db_block_size' and rownum = 1) b
                   )
select
         scan_data.inst_id
       , round(blocks_remaining*block_size/1024/1024/time_remaining,1) MB_per_Second
       , scan_data.time_remaining
       , round(time_remaining/(1-pct_scanned/100)) predicted_runtime_seconds
       , scan_data.pct_scanned
       , scan_data.blocks_remaining
       , round(blocks_remaining/(1-pct_scanned/100)) total_blocks
       , scan_data.opname
       , scan_data.BLOCK_SIZE
       , scan_data.target
       , (
          select
                  max(operation_id)||':'||DECODE(MAX(NUMBER_PASSES),0,'OPTIMAL',1,'ONE-PASS',NULL,NULL,'MULTI-PASS('||max(number_passes)||')')||DECODE(max(TEMPSEG_SIZE),NULL,NULL,','||round(max(TEMPSEG_SIZE)/1024/1024)||'M')
          from gv$sql_workarea_active
          where gv$sql_workarea_active.sid = scan_data.sid
          and gv$sql_workarea_active.inst_id = scan_data.inst_id
--          and scan_data.opname in ('Hash Join','Sort Output')
--          and gv$sql_workarea_active.OPERATION_TYPE in ('HASH-JOIN','SORT','WINDOW (SORT)','GROUP BY (SORT)')
         ) id_passes_temp
       , scan_data.sid
       , scan_data.username
       , scan_data.sql_hash_value
from scan_data
order by inst_id,username,sid,time_remaining
/
