import React from 'react';
import { LockOutlined, UserOutlined, MailOutlined } from '@ant-design/icons';
import { Button, Checkbox, Form, Input } from 'antd';
import { useAuth } from '../../context/AuthContext';

const RegisterForm = ({setIsRegistered }) => {

  const {emailError, usernameError, register } = useAuth();

  const onFinish = async (values) => {
    console.log('Received values of form: ', values);

    const registerData = {
      username: values.username,
      email: values.email,
      password: values.password,
    };
    console.log(registerData);
 
    await register(registerData);
  };

  return (
    <Form
      name="Registration"
      initialValues={{
        remember: true,
      }}
      style={{
        maxWidth: 360,
      }}
      onFinish={onFinish}
    >
      <div style={{
        display: 'flex',
        justifyContent: 'center',
        marginBottom: '1em',
        fontSize: '1.25em'
      }}>
        Registration
      </div>
      <Form.Item
        name="email"
        validateStatus={emailError ? 'error' : ''}
        help={emailError}
        rules={[
          {
            required: true,
            message: 'Please input email!',
          },
          {
            type: 'email',
            message: 'Please enter a valid email',
          }
        ]}
      >
        <Input prefix={<MailOutlined /> } placeholder="Email" />
      </Form.Item>

      <Form.Item
        name="username"
        validateStatus={usernameError ? 'error' : ''}
        help={usernameError}
        rules={[
          {
            required: true,
            message: 'Please input username!',
          }
        ]}
      >
        <Input prefix={<UserOutlined />} placeholder="Username" />
      </Form.Item>

      <Form.Item
        name="password"
        rules={[
          {
            required: true,
            message: 'Please input password!',
          },
        ]}
      >
        <Input prefix={<LockOutlined />} type="redPassword" placeholder="Password" />
      </Form.Item>

      <Form.Item>
        <Button block type="primary" htmlType="submit">
          Register
        </Button>
        or <a onClick={() => setIsRegistered(true)}>Log in</a>
      </Form.Item>
    </Form>
  );
};

export default RegisterForm;
