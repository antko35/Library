import React, { useState, useEffect } from 'react';
import { List, Avatar, Spin, Pagination, Rate, notification, Typography } from 'antd';
import axios from 'axios';

const { Text } = Typography;

const Comments = ({ bookId }) => {
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize] = useState(5); // Количество комментариев на страницу

  useEffect(() => {
    if (bookId) {
      fetchComments(currentPage);
    }
  }, [bookId, currentPage]);

  const fetchComments = async (page) => {
    setLoading(true);
    try {
      const response = await axios.get(
        `https://localhost:7040/${bookId}/${page}/${pageSize}`
      );
      console.log(response);
      const { items, totalCount } = response.data;
      setComments(items);
      setTotal(totalCount);
    } catch (error) {
      notification.error({
        message: 'Ошибка загрузки',
        description: 'Не удалось загрузить комментарии. Попробуйте позже.',
      });
    } finally {
      setLoading(false);
    }
  };

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  return (
    <div>
      {loading ? (
        <Spin tip="Загрузка комментариев..." />
      ) : (
        <>
          <List
            dataSource={comments}
            header={`${total} комментариев`}
            itemLayout="horizontal"
            renderItem={(item) => (
              <li style={{ marginBottom: '16px', borderBottom: '1px solid #f0f0f0', paddingBottom: '16px' }}>
                <List.Item.Meta
                  title={<Text strong>{item.author}</Text>}
                  description={
                    <>
                      <Rate disabled defaultValue={item.rate} />
                      <p style={{ marginTop: '8px' }}>{item.comment}</p>
                    </>
                  }
                />
              </li>
            )}
          />
          <Pagination
            current={currentPage}
            pageSize={pageSize}
            total={total}
            onChange={handlePageChange}
            style={{ marginTop: '16px', textAlign: 'center' }}
          />
        </>
      )}
    </div>
  );
};

export default Comments;
