import React from 'react';
import { Card, Col, Row } from 'antd'; 

const BookCard = ({ title, author, genre, returnDate, coverImagePath }) => {
  const availability = returnDate === null ? 'Available' : `${returnDate} will be avalibal`;

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
        </Col>
      </Row>
    </Card>
  );
};

export default BookCard;
