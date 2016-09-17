col value format a30
col name format a40
col keyword format a10

break on keyword skip 1 dup

select          decode(sign(instr(name,'dyn')),1,'dyn'
               ,decode(sign(instr(name,'size')),1,'size'
               ,decode(sign(instr(name,'pga')),1,'pga'
               ,decode(sign(instr(name,'index')),1,'index'
               ,decode(sign(instr(name,'cpu')),1,'cpu'
               ,decode(sign(instr(name,'mode')),1,'mode'
               ,decode(sign(instr(name,'optimizer')),1,'optimizer'
               ,decode(sign(instr(name,'parallel')),1,'parallel'
               ,decode(sign(instr(name,'rewrite')),1,'rewrite'
               ,decode(sign(instr(name,'statistics')),1,'statistics'
               )))))))))) keyword
      ,name
      ,value
      ,isdefault
from v$parameter
where (
        name like '%dyn%' or
        name like '%pga%' or
        name like '%area%' or
        name like '%index%' or
        name like '%cpu%' or
        name like '%mode%' or
        name like '%optimizer%' or
        name like 'parallel%' or
        name like '%rewrite%' or
        name like '%statistics%'
      )
order by keyword,name
/
