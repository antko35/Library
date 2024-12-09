import React, { useState, useEffect } from 'react';
import { List, Avatar, Spin, Pagination, Rate, notification, Typography, Form, Input, Button} from 'antd';
import axios from 'axios';

const { Text } = Typography;
const { TextArea } = Input;

const Comments = ({ bookId }) => {
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize] = useState(5); 
  const [addingComment, setAddingComment] = useState(false);

  useEffect(() => {
    if (bookId) {
      fetchComments(currentPage);
    }
  }, [bookId, currentPage]);

  const fetchComments = async (page) => {
    setLoading(true);
    try {
      const response = await axios.get(
        `https://localhost:7040/Comments/${bookId}/${page}/${pageSize}`,{
            withCredentials: true
        }
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

  const handleAddComment = async (values) => {
    const { comment, rate } = values;
    setAddingComment(true);
    try {
      const response = await axios.post(`https://localhost:7040/Comments`, {
        bookId,
        comment,
        rate,
      }, {
        withCredentials: true // позволяет отправлять куки с запросом
    });

      notification.success({
        message: "Комментарий добавлен",
        description: "Ваш комментарий успешно добавлен.",
      });

      // Обновляем список комментариев
      fetchComments(currentPage);
    } catch (error) {
      notification.error({
        message: "Ошибка добавления",
        description: "Не удалось добавить комментарий. Попробуйте позже.",
      });
    } finally {
      setAddingComment(false);
    }
  };


  return (
    <div>
        <Form layout="vertical" onFinish={handleAddComment} style={{ marginBottom: "16px" }}>
        <Form.Item
          name="comment"
          label="Ваш комментарий"
          rules={[{ required: true, message: "Пожалуйста, введите комментарий" }]}
        >
          <TextArea rows={4} />
        </Form.Item>
        <Form.Item
          name="rate"
          label="Ваша оценка"
          rules={[{ required: true, message: "Пожалуйста, поставьте оценку" }]}
        >
          <Rate />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit" loading={addingComment}>
            Добавить комментарий
          </Button>
        </Form.Item>
      </Form>
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
                    title={
                    <div >
                    <Text strong >{item.author}</Text>
                    <Text type="secondary" style={{ display: "block" }}>
                        {new Date(item.writingDate).toLocaleDateString("ru-RU", {
                            year: "numeric",
                            month: "long",
                            day: "numeric",
                        })}
                    </Text>
                    
                    </div>
                    }
                    description={
          <>
            <Rate disabled defaultValue={item.rate} />
            <p style={{ marginTop: "8px" }}>{item.comment}</p>
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
