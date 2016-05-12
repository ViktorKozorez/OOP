package object.pool;

import java.util.LinkedList;
import java.util.Queue;

public class ObjectPool<T> {
    private final Queue<T> objects = new LinkedList<>();
    
    public void checkIn(T element) {
        this.objects.add(element);
    }
    
    public T checkOut() {
        T element = this.objects.remove();
        return element;
    }
    
    public void setMaxPoolSize(int size) {
        while(this.objects.size() > size) {
            this.objects.remove();
        }
    }
}