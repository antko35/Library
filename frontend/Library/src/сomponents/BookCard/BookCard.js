import React from 'react';
import { Card, Col, Row, Button} from 'antd'; 

const BookCard = ({ title, author, genre, returnDate, coverImagePath, onBorrow }) => {
  const availability = returnDate === null ? 'Available' : `booked until ${returnDate}`;
  const isAvailable = returnDate === null;

  return (
    <Card hoverable style={{ width: "30%"}}>
      <Row gutter={16}>
        <Col span={8}> {}
          <img 
            alt='cover' 
            src={`https://localhost:7040/uploads/${coverImagePath}`} 
            style={{ width: '100%', height: 'auto' }} 
          />
        </Col>
        <Col span={16}> { }
          <Card.Meta title={title} description={`By ${author}`} />
          <p>Genre: {genre}</p>
          <p>Status: {availability}</p> { }

          {isAvailable && (
            <Button type="primary" onClick={onBorrow}>
              Borrow
            </Button>
          )}
        </Col>
      </Row>
    </Card>
  );
};

export default BookCard;
