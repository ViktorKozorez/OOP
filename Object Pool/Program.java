package object.pool;

public class Program {
    public static void main(String[] args) {
        ObjectPool<Point> pool = new ObjectPool();
        
        pool.setMaxPoolSize(3);
        
        pool.checkIn(new Point(10, 10));
        pool.checkIn(new Point(20, 20));
        pool.checkIn(new Point(30, 30));
        pool.checkIn(new Point(40, 40));
        pool.checkIn(new Point(50, 50));
        
        Point point = pool.checkOut();
        
        point.setX(20);
        point.setY(50);
        
        pool.checkIn(point);
        
        pool.setMaxPoolSize(3);
    }
}