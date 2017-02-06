

STARTS Core Coding Standard and Code Review Process

Version <0.1>    
3 Oct 2016
Document ID
Clarity Project ID:  


 
Table of Contents
Document Information	3
Coding Standard	4
Introduction	4
Coding Style	4
Coding Best Practice	9
Coding Example	13
Code Review process	16
Scope of review	16
Roles and responsibility	16
Peer Review process	16
Automation	16
Appendix	17
Comments in the past	17
Code Review Checklist	17

 
Document Information
Revision History
Date	Version	Status	Prepared by	Comments
30-Sep-2016	0.1	DRAFT	Peter Meng, Andy Wong	First Draft
				


Document Control
Role	Name	E-mail	Telephone
Owner	Peter Meng	peterlmeng@hsbc.com.cn	
	Andy Wong	andy.kai.ho.wong@hsbc.com.hk	
Reviewer (s)	Chris John Boyle	chrisjohnboyle@hsbc.com.hk	
	Laurence Hook	laurenceahook@hsbc.com.hk	
	Javen Fan	javenzffan@hsbc.com.cn	
	Danny Yuan	danny.w.h.yuan@hsbc.com.cn	
	Jack Li	jackswli@hsbc.com.hk	
			

Approval
Role	Name	Signature	Sign-off Date
Regional Head of Finance IT	Chris John Boyle		
Architect	Laurence Hook		
			
			
			


Coding Standard
Introduction
The objective of coding standard is for strengthening of code quality through increase robustness of code, enhance maintainability, support ease, performance, reusability of the code.
This document covers the coding style and coding best practices however there are some areas belongs to design area (which need to go through design forum before staring development) e.g. new table, new package to use.

Coding Style 
Purpose: Easy to read/review/maintain, less man made mistake
1.1	Naming Convention
Make sure the database objects (including package, function, table, view, type, trigger etc.) and PLSQL objects (variables, cursor etc.) are following the right naming convention as provided below.
[Table 1]
Oracle Naming Convention
	PL/SQL Object Type 		Scope/Type		Prefix/Format		Example
	Global/Local Scope		Global		g_<root>		g_my_variable
		Local 		l_<root>		l_my_variable
	Constants		Global(package)		gc_<root>		gc_max_record_length
		Local 		c_<root>		c_fetch_limit
	Variables		Global(package)		g_<root>		g_my_variable
		Local(pl/sql block)		l_<root>		l_my_variable
	Parameters		in		p_<root>		p_firmctycode
		out		po_<root>		po_countrycode
		both		pio_<root>		pio_systemcode
	Cursors				[g_]cur_<root>		cur_security_leg
	Cursor Variables				[g_]cv_<root>		cv_system_code
	Ref Cursors				rcur_<root>		rcur_cash_Leg
	Record Types				[g_]typ_<root>		typ_schedule
	Record Type Variables				[g_]rec_<root>		rec_books,g_rec_books
	Collection Types		Nested Tables		[g_]nt_<root>		nt_user,g_nt_users
		VARRYs		[g_]va_<root>		va_permissions
		Associative Arrays		[g_]aa_<root>		aa_employees
	Collection Variable				[g_]]rec_<root>		rec_permissions,g_rec_users
	Exceptions				e_<root>		e_invalid_firmctycode
	Lables				lbl_<root>		lbl_exception_block
							
	Oracle Object Type		Scope/Type		Prifix/Format		Example
	Tables		Staging table		stg_<tablename>		stg_gfdm_deal
		Data warehouse table		dw_<tablename>		dw_d_trade
		Data Mart Table		dm_<tablename>		dm_f_balance
		Data Controller table		dc_<tablename>		dc_node_instance
		Report Table		rpt_<tablename>		rpt_otc_dd
		Working Tables		<component abbr>_wrk_<tablename>		dw_wrk_latest_trade
		Helper table		<component abbr>_h_<tablename>		stg_h_source_system
	Views		View		<component abbr>_v_<view name>		dc_v_node_monitor
		Materialised view		<component abbr_<view name>		dw_mv_structure_trade
	Constraints		Primary key		pk_<tablename>		pk_mtn_site_config
		Foreign key		fk_<child_table>_<parent_table>_<seq>		fk_tlog_tloglevel_01
		Unique keys		uk_<tablename>_<seq>		uk_hclpr_01
		Check constraint		c_<tablename>_<column>		c_tlog_country
	Indexes		Unique index		idxu_<tablename>_<seq>		idxu_mtn_broker_01
		Non-unique index		idx_<tablename>_<seq>		idx_stn_tts_mmswbtp_01
		Foeign-Key Index		idx_fk_<tablename>_<seq>		idx_fk_f_balaid_boo_r
	Sequences				sq_<tablename>		sq_tlog
	Triggers				trg<_<tablename>_<seq>		trg_tlog_01
	Functions				f_<function name>		f_get_process_date
	Procedures				p_<procedure name>		p_load_gfdm_trade
	Packages				pg_<package name>		pg_load_gfdm_dimensions

Use the full name for the column naming convention, but if the column name is too long to use the full name, use the abbreviation as below:

[Table 2]
System specific naming conversion
	Name		Abbreviations 		Remark
	account		acc		　
	agreement		agmt		　
	alternative 		alter		　
	Consolidated		consol		　
	counterparty		cpty		　
	currency		ccy		no need to add "code" in the field name, ccy stands for "currency code" already
	current		curr		　
	description		desc		　
	exchange		exch		　
	external		ext		　
	flag		flag		Binary Value, two possible vaules : Y or N only
	global		gbl		　
	Indicator		ind		　
	initial		init		　
	internal		int		　
	local		lcl		　
	location		loc		　
	management		mgmt		　
	number		no		　
	physical		phy		　
	product		prod		　
	receive		rec		　
	reference		ref		　
	standard		std		　
	structure		stru		　
	settlement		settle		　

1.2	Case convention to aid readability
Make sure all keywords, reserved words and references to Oracle supplied packages, procedures and functions in Uppercase, others in lowercase, not follow the rules will need an explanation.
This would make the code reading more easily. 
Example: SELECT * FROM dw_d_trade WHERE 1=0;
1.3	Use meaningful alias and use these alias when select fields 
Use meaningful alias when you dealing with more than one table (or view), and when selecting columns, use alias as prefix no matter what. This would make reading of the query easier and reduce the chance of making mistake. 
Example: SELECT trade.id_trade, book.department_code FROM dw_d_trade trade [INNER|LEFT OUTER|RIGHT OUTER] JOIN dw_d_book book ON trade.id_book = book.id_book;
1.4	Comments
Make comments in the code only when it’s necessary, don’t comment on code block that can explain itself.
For example, in a function named <get_country_code>, you don’t need to comment as “This function is to get the country code from dw_d_counterparty table”. Do it when the logic is hard to follow, but still try to keep it short.
Please note, DO NOT comments out large chunk of code in you deliver version, not doing so will create lots of noise and make it hard to review.
Example:  
1.5	Separate the logic of joining datasets from the logic of filtering
We have observed that additional criteria are added to a normal JOIN (INNER JOIN) clause to give effect of a filter, this mixture style is not a good practice because you might overlook some filters due to this in a very long query. 
However, pay attention to the OUTER JOIN, DO NOT add filter on driven-to table in the where clause because it would make the OUTER JOIN act as INNER JOIN, so keep it in the ON clause.
1.6	Use ANSI way to join tables (or views) in the query 
Although in most cases the execution paths for equivalent ANSI and Oracle syntax queries will be the same (exception being the FULL OUTER JOIN), the advantage of adopting the ANSI JOIN syntax is code readability and maintainability. It can be argued that using ANSI JOIN syntax increases the amount of code written, however the gains that are made in code readability far out-weigh this disadvantage as it allows one to separate the logic of joining datasets from the logic of filtering of datasets.
Example: 
SELECT DISTINCT
       e.id_entity,
       SUBSTR (org_code, 1, 3) AS department_code,
       SUBSTR (org_code, 4, 2) AS section_code,
       CASE WHEN accounting_method_type = 'T' THEN 'T' ELSE 'B' END
          AS banking_trading_indicator
FROM stg_v_gfdm_deal d
       JOIN dw_d_entity e
              ON l_correct_country_code = e.country_code
             AND d.group_member = e.group_member
             AND d.company_code = e.branch_number
WHERE d.business_date = ctr.business_date AND d.site_code = ctr.site_code;
1.7	Remove unused parameter from the code
Remove unused variables from programs, sometime this may cause bugs if, for example, you assigned a value to the wrong variable and don’t even notice it.
1.8	Data type mapping: Use %TYPE whenever possible
Using %TYPE to anchor the data type ensures that your variables stay synchronized with your database structure, avoiding type mismatches and future proofing it in case the underlying column changes. Also, it makes the code more self-documenting. Anyone reading it will see that there is some relationship between your variable and the database column, making it easier to understand it, make changes, and spot potential bugs. 
Example: 
PROCEDURE log(
    p_text          IN dw_log.text%TYPE,
    p_section       IN dw_log.section%TYPE  ,
        p_level         IN dw_log.log_level%TYPE )
1.9	Adding new function/procedure 
Please note DO NOT create new function/procedures similar to existing ones that servers for the same purpose, reuse them instead. 
1.10	No copy and paste
When you see large chunks of code repeated and cut and paste with very minor changes, consider write a function or procedure with proper parameters to replace it. This would make the program more readable and also avoid the mistake we may make while copy them around, so modularize, centralize and share code where ever possible.
1.11	Use indentation properly
When formatting the code, remember to use the proper indentation. It will make the code cleaner and easier to read/review. All code should be indented using a tab of three spaces. Column listings should have the comma in front of the separated element and the coma is right aligned.
1.12	Use formatting tools for other formatting points
As most of our developers using SQL Developer as coding tools, adjust it for use.
Using the tools to do code formatting is very simple, take SQL Developer as example:
1.	Select all the code you want to format, and right click the code to choose “Formatting” option
2.	You can edit the code format template in Tools  Preference  Database  SQL Formatter  Oracle Formatting, click Edit to setup the format like Indentation, Uppercase/Lowercase, Alignment, Case Line breaks etc.
I’ll prepare a format template for Toad (as it’s more robust and format is cleaner) later so everyone can use it to do the format in the same way.
1.13	Hard Code 
We should avoid to use hard code in our program, for example, you should enhance reference tables e.g. STG_H_SOURCE_SYSTEM, STG_H_SPARC_REPORTING_UNIT (new), so no need to hardcore values into code.  But under some cases, it’s necessary and also make sense to use hard code, like in the SQL statement you need to filter out the data with pay_or_receive flag equal to “P”. 
Coding Best Practice 
Purpose: Good Performance, Easy to debug
1.14	Consider using Package 
Where possible all code (functions and procedures) should be contained in a package. Packages offer a number of advantages. The most important of these are:
1.14.1	Decoupling of dependent procedures. Allows for independent code compilations without invalidating dependent code modules.
1.14.2	Sharing of common components such as constant values, common record types.
1.14.3	Marginal performance gains.
1.15	Add Error handling code for easy analysis
We should add exception handling code in every procedure/function. 
1.15.1	We built the pg_log.error to help record the error message captured in the program and it should be called as below in the exception handling section. 
1.15.2	Remember to rollback or release lock or close the cursor to release the resources the session acquired.
1.15.3	Remember to format the error message using oracle defined package dbms_utility as below, it would show the detailed error message and the line in which the failure happen.
1.15.4	Remember to raise the exception to stop the processing unless you have a good reason that the exception can be logged only and move on to next step. 
Example:
EXCEPTION
WHEN OTHERS THEN
      rollback;
      pg_log.error( dbms_utility.format_error_stack || chr(10) || dbms_utility.format_error_backtrace);
      pg_lock.release_lock;
          raise_application_error(-20000,ctr.business_date||ctr.site_code||dbms_utility.format_error_stack || chr(10) || dbms_utility.format_error_backtrace);
1.16	Log the time of each step in the program using pg_log.info
We have developed a sophisticate framework to allow up record the time of each step in a program, so whenever we add a query or a function/procedure call in the program, we need to log it. The detail will be showed in the dw_log table. It can help us quickly locate the bottleneck of the performance for everything happened in the database.
Example:
 
1.17	Things to consider when create new table or add new fields to existing table
Before creating, remember to design your table properly by using primary key, constraint, index, partition to define your preferred access path when you use it, make good use of constraint to ensure the code integrity. Things to consider:
1.17.1	Clearly define the column if NULL allows or not when create new tables
1.17.2	Primary key is required for most of our tables (without that needs explanation)
1.17.3	staging tables and balance tables are partitioned using composite partition method ( range-list, range by business_date, list by site_code ) and when doing selecting need to make sure partition pruning works ( only scan the necessary sub-partition ).
1.17.4	Children tables with foreign key refers to other table need to have index created on these foreign key columns.
1.17.5	When adding new fields, evaluating the existing indexes of the table to determine if the new fields need to be considered to add to the index.
1.18	Use sub query carefully
Recently we had a case that sub query was used in the select statement hidden in a function call in the balance loading. A better way to do so is to add this table as a join. Sub query in the select statement will cause performance issue especially when the sub query doesn’t have the proper unique index created based on the filter.
1.19	Use WITH AS to improve the performance 
Organize complex SQL statements using features such as sub query factoring (WITH ...) and the QB_NAME hint to break them into logical components, this will be helpful when trying to manage the performance of a complex query. 
1.20	Avoid to use hints UNLESS it’s necessary
Hints are useful in some cases but should be very careful because the hints may not be appropriate as time passed (table structure may change, new indexes may be created in future). Only use it when you think it’s necessary and there is no other way to tune the performance.
1.21	Type conversion
Always use explicit type conversions (example: TO_DATE('20150101','YYYYMMDD') and never rely on implicit conversion.
1.22	Result Cache function
Think carefully before make the function as result cache function, use it when the function returns limited records and get called by many other applications with similar/same criteria. Otherwise you will mess up the result cache in SGA (i.e. heavy contention on result cache latch) and make the performance worse. 
1.23	Use bulk collection to improve the performance
Possible use case: The source data needs some complex pre-processing/logic transformation before loading into a core table. Read the package in pg_load_balance for more details regarding how to use it.  
1.24	Use MERGE to improve the performance
Any data merging should be done via set-based MERGE and not the old PL/SQL technique of attempting either an insert first and updating on failure or attempting an update first and inserting when the update affects no rows. The following techniques should be used wherever possible:
Method	When? 
Standard merge
(MERGE .. INTO .. USING ..)	When the source data (generated from a staging table, collection or SQL statement) can be directly merged into the core table with little complexity.
Merge from pipelined function
(MERGE .. INTO .. USING (TABLE(pipelined_function)))	When the source data requires some complex transformation to prepare it for merge.
Bulk PL/SQL 
(FORALL .. MERGE)	MERGE is a set operation. This technique should really be a last resort (in fact, there should never be a good reason to use it). Is still quicker than row-by-row PL/SQL method.

1.25	Ensure the partition pruning works as expected to ensure good performance 
The key of the good performance in our system is to make sure we only scan the data that’s necessary. All of our staging tables, DD tables and also balance table are partitioned by date and sub partitioned by site, when query from these tables, we need to make sure we scan only one sub partition for that particular day and site by adding the right business date and site code as filter. The execution plan for partition pruning looks like below:
  
1.26	Make use of DML error logging
We used this a lot in our program, a corresponding error logging table was created when we load data into our target tables. This is extremely useful with some guidelines for using this feature:
1.26.1	The optional tag should uniquely identify any set of errors and should ideally be unique to a specific data load. The tag can be a variable or bind variable and the value will be stored with the errors.
1.26.2	Performance can sometimes degenerate with large data loads, particularly in conventional path, so there might be critical loads where DML error logging will need to be removed.
1.27	Use result cache hint
 The Query Result Cache should only be used for discrete queries with small result sets that are executed many times. Queries that generate large result sets should NOT not use the Query Result Cache.  
Example: SELECT /*+ RESULT_CACHE */ SOURCE_SYSTEM, MAX (PROCESS_EOD) FROM CORB3_NCOM_COLLATERAL WHERE PROCESS_EOD <= :B1 GROUP BY SOURCE_SYSTEM;
1.28	Program/Architect related A: Logic in different layer
 CAPITAL specific reporting logic should be applied report layer instead of standardization layer. The principle here is business logic must apply to the right layer as we have now.
1.29	Program/Architect related B: Logic transformation using PLSQL 
If using CURSOR…BULK COLLECT…FOR ALL way to do logic transformation, make sure to select the minimum fields in the CURSOR and do the logic transformation/supplementation in the transformation section.
1.30	Program/Architect related C: Table Logging/NoLogging and Compress
 Our tables need to be created with nologging and oltp compress for the purpose of improving the performance when loading and selecting because compare with loading, we have more reading in our system. Like our staging table, we load once but select it in many other difference places. We can benefits from the table compress with better select performance and also reduce the disk usage. As we have the ability to rerun every node/file as long as the source files are there, we can make use of nologging to improve the performance of data loading.
Keywords: NOLOGGING COMPRESS FOR OLTP
1.31	Program/Architect related D: Add proper audit 
Audit the dimension tables’ new added field
Remember to audit the new added filed for dimension tables.
 
Coding Example 
We have built a set of examples that covered examples like package, view, table, sequence, etc. and also include test cases to self-prove. 
This will help us understand how our database framework works and also a very good reference for new joiners to learn how to build their own program based on the business requirement. We have different layers and each layer has its own coding pattern – these codes were wrapped into a package. Below will go through these package one by one as they are very important and also the key to understand the whole framework.
Before the go through, there is a basic unit we need to understand, the CTR, short for pg_complext_tranformation, this is a package and it need to be called in every procedure/function, ctr.setup need to be called at the first beginning of the procedure/function, ctr.finish is called after all the works done in the procedure/function.
It’s the fundamental of our logging system. 

1.32	PG_STAGING_EXAMPLE package 
 
This package servers for staging layer to get the data loaded into our staging tables. There are several things need to take care of: prepare work before loading, loading process, post process after loading, undo process( if needed), rollover process(if needed) and housekeeping. For most of the cases, each one of our staging table will have a represent its own staging package. 
Get_arrary function is a pipeline function used to get dataset from array passed from Java objects and represent it as a table for the loading. 
Pre-process procedure will add the proper partition based on the business day and be able support re-run. 
Process procedure is where the real loading take place, it get data from pipeline function get_arrary and load it into the target table.
Post_process procedure will gather the statistics for the staging table and identify the master records for further use.
Undo procedure is used to drop the data for the specific business date and site.
Rollover procedure is called when there is a holiday for a site, it will copy the last day’s data into current business day’s sub partition.
Housekeeping is used to do data housekeeping based on the retention day. 
Real use case:
 
1.33	PG_LOAD_DIMENSION_EXAMPLE package
This package is used to loading the dimension tables in dw layer, like dw_d_trade, dw_d_book, etc. 
There are several things need to pay attention to:
1.33.1	Use pg_lock to prevent the deadlock.
Because dimension tables have primary/foreign key constraints defined, each site (we have 22 sites) will try to update/insert it and big chances there are more than 2 sites trying to update it at the same time, it will cause deadlock. So before the loading, we need to call the pg_lock package to make sure the updating/inserting happen in a serial way. Also, remember to release the lock after loading.
Sample usage: pg_lock.get_lock(p_table => c_table);
3.2.2 Use instead of trigger on the view for audit purpose 
3.2.3 Use global temporary table to capture the latest data
1.34	PG_LOADING_FACT_EXAMPLE package
This package is used in dw layer to load the fact table, like dw_f_balance. 
We do the fact loading using CURSOR...FOR ALL… LOOP…<Transformation> LOAD pattern, the reason to do so is that FACT loading in our system contains lots of logic transformation, doing this through a pure SQL statement make it hard to maintain and also the performance is not controllable.
1.35	PG_LOAD_REPORT_EXAMPLE package 
We have built the report view which joins the FACT table and dimension tables together.
This package servers for report layer, which load report into the table from the view.
The whole process is pretty simple as showed below, don’t forget to use APPEND to improve the loading performance, and in order to use APPEND we need to specify the sub partition name, otherwise the APPEND will lock the whole table and prevent other sessions to load data into their own sub partitions.
 
1.36	PG_IMPORT_UTITLY_EXAMPLE package
This package is created to wrap all import related functions/procedures to aid the process in every layer.
 
 
Code Review process
Scope of review
-	In scope for new code only, existing code is not in review scope however if found any non-compliance can raise as defects to resolve separately.
-	To ensure built code is aligned with coding standard instead of ensuring result correctness (this should be proved through test evidence provided by developers)
Roles and responsibility
-	Peer reviewer is to cover the review of code according to coding standard
-	Experienced STARTS developers to do peer review 
-	Component owner approval is to ensure completion of code review and development work, not to do code review 
-	Advisory role to provide input to coding standard and review criteria
Peer Review process
-	Timeliness – reviewer need to check review list every day otherwise if a task if pending for too long then when other developer checks in new code may cause conflict and need to do code merge then and do the review again.
-	This document of coding standard is for code reviewer to take as basis for review
-	Having said that, one page checklist as summary for reviewer is prepared to complete the review (in Appendix)
-	Reviewer to include comment during review (served by completing checklist)
-	Will deliver briefing session and regular refresh
-	Peer review does not mean get QA to review your results. It’s a CODE or ANALYSIS review.
-	Ready for Review means it has been tested by the developer himself, meaning unit testing, CI build, is being done and completed without issue, and test evidence should be attached into RTC.
-	If the Ready for Review item is a new feature with significant change in UI / system flow, please be prepared to DEMO or at least walk through your changes with the reviewer. Or simply attach a screen capture during your own testing, that will be very helpful for the review to get an idea on what’s the change is about.
-	Defect fix should have a clear explanation of the issue and the resolution with evidence attached.
Automation 
-	tried the sonar, the rules defined in it are very simple and some of them does not that make sense as Peter went through with Danny/Jimmy before. But we may create rules by ourselves in future. Subject to further exploration.

 
Appendix
Comments in the past 

•	Query to balance table has become snowflake rather than star.
•	New fields added and indexing not considered 
•	PLSQL hides sub selects inside functions (never do sub queries in the select clause)
•	Functions use result cache feature when they have many many inputs variations and are called only a few times
•	additional criteria and added to a join clause to give effect of a filter. Filters should be in where clause and when it is not obvious what they it should be commented
•	capital specific reporting logic applied in standardisation layer 
•	standardisation and mapping logic applied in reporting layer
•	large chunks of code commented out in delivered code
•	large chunks of code repeated and cut and paste with very minor changes (rather than make a parameterised proc)
•	In balance etc transformations added to data collection cursor rather than in the transformations section (the bulk collect loop)
•	insert or update statement added but no log call added

Code Review Checklist
[Table 3]
Area	Checklist Item	Description	Passed?
Coding Style	Naming Convention	Make sure the database object name, column name, parameter name etc. following the right name conversion per the list provided in the coding standard document	Y
	Case convention	All keywords, reserved words and references to Oracle supplied packages, procedures and functions in Uppercase, others should be lowercase, not follow the rules will need an explanation.	Y
	Alias	Use meaningful alias when you dealing with more than one table (or view), and when selecting columns, use alias as prefix no matter what.	Y
	Comments	Using it only necessary and make it short if doing so	Y
	Separate the join from filter	Separate the logic of joining datasets from the logic of filtering	Y
	Join Way	Use ANSI way to join tables (or views) in the query	Y
	Unused parameter	Remove unused variables from programs	Y
	Data type mapping	Use %TYPE whenever possible to  avoided type mismatches and future proofing it in case the underlying column changes	Y
	No copy and paste	Make a standard interface instead of copy same block of code around	
	Adding new functions/procedures	DO NOT make new function/procedures similar to existing ones that servers for the same purpose.	Y
	Code Indentation	All code should be indented using a tab of three spaces. 	Y
	Hard code	Avoid to use hard code, consider to use a configuration table to replace it. Use it only necessary as explained in section 1.13 	Y
Coding Best Practice	Use package as preferred 	Use package instead of standalone procedure/function	Y
	Error handling	Add exception handling for each of the procedure/function	Y
	Time/Event Logging 	Use pg_log.info to log event and elapsed time for every step of your program	Y
	New table / New fields	Get approval from design team first, then think carefully for the naming conversion/primary key/index/partition/constraints/no logging/compression before create	Y
	Sub Query	Check if it's used properly per the standard document	N/A
	Hints	Use it only when there is no other way to tune the query	Y
	Type Conversion 	Always use explicit way to do the conversion	Y
	Merge	Consider use the Merge to tune your query, i.e. update/insert to the same table	N/A
	Partition Pruning 	Ensure the partition pruning works as expected for performance 	Y
	DML Error Logging	User Error table if you want to capture the "dirty" data only instead of making it stop by throwing exception	Y
	Result Cache	Use it when limited records returned and wildly called with minor parameter change 	Y
	Logic layer	Business logic must apply to the right layer as we have now	Y
	Audit	Remember to audit the new added filed to the dimension tables	Y

 
