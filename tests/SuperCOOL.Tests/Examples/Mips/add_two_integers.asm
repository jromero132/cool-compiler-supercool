# Hello { name }
.data
msg1: .asciiz "Enter a value: "
msg2: .asciiz "Enter another value: "
msg3: .asciiz "Sum is: "

.text
main:

# Print msg1
li $v0, 4
la $a0, msg1
syscall

# Read first int
li $v0, 5
syscall

# Save int to register
move $s0, $v0

# Print msg2
li $v0, 4
la $a0, msg2
syscall

# Read second int
li $v0, 5
syscall

# Add both ints
add $s0, $s0, $v0

# Print msg3
li $v0, 4
la $a0, msg3
syscall

# Print result
li $v0, 1
move $a0, $s0
syscall

# Exit
li $v0, 10
syscall