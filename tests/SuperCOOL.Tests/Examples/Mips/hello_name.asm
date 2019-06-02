# Hello { name }
.data
question: .asciiz "What's your name?\n"
hello: .asciiz "Hello "
buffer: .space 20

.text
main:

# Print question
li $v0, 4
la $a0, question
syscall

# Read name
li $v0, 8
la $a0, buffer
li $a1, 20
syscall

# Print "Hello"
li $v0, 4
la $a0, $a1
syscall

# Print { name }
li $v0, 4
la $a0, buffer
syscall

# Exit
li $v0, 10
syscall