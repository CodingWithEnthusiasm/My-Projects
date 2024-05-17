
section .bss
    operation resb 2 ; Buffer to store the input character ( with size of operation 1, so we can store only one variable)
    number1 resd 2; reserving 4 bytes for input of first variable ( reserve number of  doublewords)
    number2 resd 2; reserving 4 bytes for input of second variable ( ( reserve number of  doubleword)
    res resb 2; result // екн куздфсштп цшер 4
    
section .data
    operation_selection db 'Select operation out of (+, -, *, /):',10, 0 ; Null terminated string for the operation_selection (0 marks the end of the string.)
    operation_selection_length equ $-operation_selection
    
    input_number1_msg db 10,'Enter first number:',10,0 ; 20 ( including endline character. ( size of this str))
    input_number1_length equ $-input_number1_msg
    
    input_number2_msg db 10,'Enter second number:',10,0 ; 21 ( including endline character. ( size of this str))
    input_number2_length equ $-input_number2_msg

    plus_msg db 10,'You selected +', 10, 0 ; Message for the '+' case
    plus_length equ $-plus_msg

    minus_msg db 10,'You selected -', 10, 0 ; Message for the '-' case
    minus_length equ $-minus_msg
    
    multiply_msg db 10,'You selected *', 10, 0 ; Message for the '*' case
    multiply_length equ $-multiply_msg

    
    divide_msg db 10,'You selected /', 10, 0 ; Message for the '/' case
    divide_length equ $-divide_msg


    result_msg db 'Result:', 10, 0; result message
    result_length equ $-result_msg; result message

    default_msg db 'Invalid operation', 10, 0 ; Message for an invalid operation
    default_msg_length equ $-default_msg
    
section .text
    global _start

    
_start:
    ; Print the operation_selection
    mov eax, 4 ; sys_write system call number
    mov ebx, 1 ; file descriptor 1 (stdout)
    mov ecx, operation_selection ; address of the operation_selection string ( len(operation_selection) + endl char)
    mov edx, operation_selection_length ; length of the prompt string + endline
    int 0x80 ; interrupt to invoke system call

    ; Read a character from stdin (file description 0)
    mov eax, 3 ; sys_read system call number
    mov ebx, 0 ; file descriptior 0 (stdin)
    mov ecx, operation ; operation to store the input character
    mov edx, 1  ; number of bytes to read
    int 0x80 ; interrupt to invoke system call
    
    ; Read two numbers one by one:
    
    ; Read first number:
    mov eax, 4 ; sys_write system call number ( 4 )
    mov ebx, 1 ; file descriptor 1 (stdout)
    mov ecx, input_number1_msg ; address of the operation_selection string ( len(operation_selection) + endl char)
    mov edx, input_number1_length ; length of the prompt string + endline
    int 0x80 ; interrupt to invoke system call
    
    ; Read a first number from stdin (file description 0)
    mov eax, 3 ; sys_read system call number
    mov ebx, 0 ; file descriptior 0 (stdin)
    mov ecx, number1 ; operation to store the input character
    mov edx, 4  ;  maximum number of bytes to read
    int 0x80 ; interrupt to invoke system call
    
    mov eax, 4 ; sys_write system call number ( 4 )
    mov ebx, 1 ; file descriptor 1 (stdout)
    mov ecx, input_number2_msg ; address of the operation_selection string ( len(operation_selection) + endl char)
    mov edx, input_number2_length ; length of the prompt string + endline
    int 0x80 ; interrupt to invoke system call
    
        ; Read a first number from stdin (file description 0)
    mov eax, 3 ; sys_read system call number
    mov ebx, 0 ; file descriptior 0 (stdin)
    mov ecx, number2 ; operation to store the input character
    mov edx, 4  ;  maximum number of bytes to read
    int 0x80 ; interrupt to invoke system call
    
    ; Switch statement ( if operation )
    cmp byte [operation], '+'
    je plus_case
    
    cmp byte [operation], '-'
    je minus_case
    
    cmp byte [operation], '*'
    je multiply_case
    
    cmp byte [operation], '/'
    je divide_case
    
    ; If none of the cases match jumpo to the default case.
    jmp default_case
    

plus_case:
    ; Print the plus message
    mov eax, 4 ; sys_write system call number
    mov ebx, 1 ; file descriptor 1 (stdout)
    mov ecx, plus_msg ; address of the plus_msg string
    mov edx, plus_length ; length of the plus_msg string + endline
    int 0x80 ; interrupt to invoke system call
    
    mov al, [number1]
    sub al, '0'
    
    mov bl, [number2]
    sub bl, '0'

    mov eax, 4
    mov ebx, 1
    mov ecx, res
    mov edx, 2
    int 0x80

    mov [res], al

    jmp print_result

    jmp end_program
    
minus_case:
    ; Print the minus message
    mov eax, 4 ; sys_write system call number
    mov ebx, 1 ; file descriptor 1 (stdout)
    mov ecx, minus_msg ; address of the minus_msg string
    mov edx, minus_length ; length of the minus_msg string + endline
    int 0x80 ; interrupt to invoke system call

    mov al, [number1]
    sub al, '0'

    mov bl, [number2]
    sub bl, '0'

    sub al, bl
    add al, '0'
    mov [res], al
    push res

    jmp print_result

    jmp end_program
    
multiply_case:
    ; Print the multiply message
    mov eax, 4 ; sys_write system call number
    mov ebx, 1 ; file descriptor 1 (stdout)
    mov ecx, multiply_msg ; address of the plus_msg string
    mov edx, multiply_length ; length of the plus_msg string + endline
    int 0x80 ; interrupt to invoke system call

    mov al, [number1]
    sub al, '0'

    mov bl, [number2]
    sub bl, '0'

    mul bl
    add al, '0'

    mov eax, 4
    mov ebx, 1
    mov ecx, res
    mov edx, 2
    int 0x80

    jmp print_result

    jmp end_program


    
divide_case:
    ; Print the minus message
    mov eax, 4 ; sys_write system call number
    mov ebx, 1 ; file descriptor 1 (stdout)
    mov ecx, divide_msg ; address of the minus_msg string
    mov edx, divide_length ; length of the minus_msg string + endline
    int 0x80 ; interrupt to invoke system call

    mov ah, 0
    mov al, [number1]
    sub al, '0'
    
    mov bl, [number2]
    sub bl, '0'

    div bl
    add ax, '0'

    mov [res], ax

    mov eax, 4
    mov ebx, 1
    mov ecx, res
    mov edx, 2
    int 0x80

    jmp print_result

    jmp end_program    
    
default_case:
    ; Print the default message
    mov eax, 4 ; sys_write system call number
    mov ebx, 1 ; file descriptor 1 (stdout)
    mov ecx, default_msg ; address of the default_msg string
    mov edx, default_msg_length ; length of the default_msg string + endline
    int 0x80 ; interrupt to invoke system call
    jmp end_program

    
print_result:
    ; Print the result_msg
    mov eax, 4 ; sys_write system call number
    mov ebx, 1 ; file descriptor 1 (stdout)
    mov ecx, result_msg ; address of the result_msg string
    mov edx, result_length ; length of the result_msg string
    int 0x80 ; interrupt to invoke system call
        
    mov eax, 4 ; sys_write system call number
    mov ebx, 1 ; file descriptor 1 (stdout)
    mov ecx, res ; buffer containing the integer to print
    mov edx, 1 ; length of the integer (assuming a 32-bit integer)
    int 0x80 ; interrupt to invoke system call
    
    jmp end_program



end_program:
    ; Exit the program
    mov eax, 1 ; sys_exit system call number
    xor ebx, ebx ; exit code 0
    int 0x80 ; interrupt to invoke system call
    


