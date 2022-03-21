# tiny-orm
A very small, simple and limited orm that will translate query language to SQL.

**Grammar**

code ->  'def' 'tables' table-def table-defs 'end' 'def' 'relationships' relationships 'end' queries
	  
relationships ->  id '.' relationship relationships
			  | E

relationship -> many '(' id ')' ';'
			  | one '(' id ')' ';'
			  
table-defs -> table-def table-defs
			| E
			
table-def -> id '{' table-cols '}'
table-cols -> table-col table-cols
table-col -> '[' table-metadata-col
			| id ':' type ';'

table-metadata-col -> 'primary'']' id ':' type ';'

queries -> id '.' query queries
		 | E
		 
query ->  insert
		| 'update' '(' json ')' update
		| 'select' '(' args ')' select
		
json -> '{' json-elements-opt '}'
json-elements-opt -> json-elements
				   | E
				   
json-elements -> json-element json-elements-prime
json-elements-prime -> ',' json-element json-elements-prime
					| E
json-element -> id ':' logical-or-expr



insert -> 'add' '(' json ')'';'

update ->  ';'
	    |  '.' 'where' '('logical-or-expr')'  ';'
		
select -> ';'
	    |  '.' 'where' '('logical-or-expr')'  ';'
		
type -> int
	  | string
	  | float
	  | bool
	  
args -> id args'
args' -> ',' id args'
	    | E
