ALTER TABLE persona
ALTER COLUMN fecha_nacimiento TYPE date
USING fecha_nacimiento::date;

ALTER TABLE persona
ALTER COLUMN fecha_nacimiento TYPE date
USING fecha_nacimiento::date;

ALTER TABLE public.persona 
ALTER COLUMN fecha_registro TYPE date;

ALTER TABLE public.cliente  
ALTER COLUMN fecha_alta TYPE date;


SELECT column_name
FROM information_schema.columns
WHERE table_name = 'Producto';

ALTER TABLE public."Producto" RENAME TO producto;

ALTER TABLE public.categoria 
RENAME COLUMN Descripccion TO  Descripcion;

-- Insertar métodos de pago básicos
INSERT INTO public.metodo_pago (nombre) VALUES
('Efectivo'),
('Tarjeta de crédito'),
('Tarjeta de débito'),
('Transferencia bancaria'),
('Pago en línea (PayPal)'),
('Cheque'),
('Criptomonedas (Bitcoin, Ethereum)'),
('Pago contra entrega');

