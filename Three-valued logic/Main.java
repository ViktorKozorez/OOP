package com.company;

public class Main {

    public static void main(String[] args) {
        Ternary F = new Ternary(State.FALSE);
        Ternary U = new Ternary(State.UNKNOWN);
        Ternary T = new Ternary(State.TRUE);

        Ternary result = Ternary.NOT(Ternary.NOT(F));
        System.out.println(result);

        result = Ternary.AND(U, U);
        System.out.println(result);
    }
}
