import React from 'react';
import { Card, Col, Row,Button } from 'antd'; 

const MyBookCard = ({ title, author, genre, returnDate, coverImagePath, onReturn }) => {
  const availability = returnDate === null ? 'Available' : `Booked until ${returnDate}`;
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
          <p>Status: {availability}</p>

          {!isAvailable && (
            <Button type="primary" onClick={onReturn}>
              Return
            </Button>
          )}
        </Col>
      </Row>
    </Card>
  );
};

export default MyBookCard;
