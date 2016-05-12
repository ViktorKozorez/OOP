package com.company;

public class Ternary {
    private State value;

    public Ternary(State value) {
        this.value = value;
    }

    public State value() {
        return this.value;
    }

    @Override
    public String toString() {
        return this.value().toString();
    }

    public static Ternary NOT(Ternary a) {
        if(a.value() == State.FALSE)
            return new Ternary(State.TRUE);
        else if(a.value() == State.TRUE)
            return new Ternary(State.FALSE);
        else
            return new Ternary(State.UNKNOWN);
    }

    public static Ternary AND(Ternary a, Ternary b) {
        if(a.value() == State.FALSE || b.value() == State.FALSE)
            return new Ternary(State.FALSE);
        else if(a.value() == State.TRUE && b.value() == State.TRUE)
            return new Ternary(State.TRUE);
        else
            return new Ternary(State.UNKNOWN);
    }

    public static Ternary OR(Ternary a, Ternary b) {
        if(a.value() == State.TRUE || b.value() == State.TRUE)
            return new Ternary(State.TRUE);
        else if(a.value() == State.FALSE && b.value() == State.FALSE)
            return new Ternary(State.FALSE);
        else
            return new Ternary(State.UNKNOWN);
    }

    public static Ternary IMP(Ternary a, Ternary b) {
        if(a.value() == State.FALSE || b.value() == State.TRUE)
            return new Ternary(State.TRUE);
        else if(a.value() == State.TRUE && b.value() == State.FALSE)
            return new Ternary(State.FALSE);
        else
            return new Ternary(State.UNKNOWN);
    }
}
