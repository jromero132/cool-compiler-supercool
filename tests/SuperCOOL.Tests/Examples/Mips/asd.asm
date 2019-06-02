.data
newline: .ascii "\n"
.text
main:
li $v0, 5
syscall
move $t0, $v0
li $v0, 5
syscall
move $t1, $v0
move $a0, $t0
add $a0, $a0, $t1
li $v0, 1
syscall
li $v0, 4
la $a0, newline
syscall
move $a0, $t0
sub $a0, $a0, $t1
li $v0, 1
syscall
li $v0, 4
la $a0, newline
syscall
move $a0, $t0
mul $a0, $a0, $t1
li $v0, 1
syscall
li $v0, 4
la $a0, newline
syscall
move $a0, $t0
div $a0, $t1
move $a0, $lo
li $v0, 1
syscall
li $v0, 10
syscall
