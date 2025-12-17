// Vercel Serverless Function for Contact Form
import nodemailer from 'nodemailer';

export default async function handler(req, res) {
  // Enable CORS
  res.setHeader('Access-Control-Allow-Credentials', true);
  res.setHeader('Access-Control-Allow-Origin', 'https://my-profile-three-theta.vercel.app');
  res.setHeader('Access-Control-Allow-Methods', 'GET,OPTIONS,PATCH,DELETE,POST,PUT');
  res.setHeader('Access-Control-Allow-Headers', 'X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Content-Type, Date, X-Api-Version');

  if (req.method === 'OPTIONS') {
    res.status(200).end();
    return;
  }

  if (req.method !== 'POST') {
    return res.status(405).json({ message: 'Method not allowed' });
  }

  try {
    const { name, email, company, interestType, message } = req.body;

    // Validate required fields
    if (!name || !email || !message || !interestType) {
      return res.status(400).json({ message: 'Missing required fields' });
    }

    // Create transporter
    const transporter = nodemailer.createTransporter({
      service: 'gmail',
      auth: {
        user: process.env.EMAIL_USERNAME,
        pass: process.env.EMAIL_PASSWORD
      }
    });

    // Email to you (notification)
    const notificationEmail = {
      from: process.env.EMAIL_FROM,
      to: process.env.EMAIL_TO,
      subject: `New Contact Form Submission - ${interestType}`,
      html: `
        <div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
          <h2 style="color: #667eea; border-bottom: 2px solid #667eea; padding-bottom: 10px;">
            New Contact Form Submission
          </h2>
          
          <div style="background: #f8f9fa; padding: 20px; border-radius: 8px; margin: 20px 0;">
            <h3 style="margin-top: 0; color: #495057;">Contact Details</h3>
            <p><strong>Name:</strong> ${name}</p>
            <p><strong>Email:</strong> <a href="mailto:${email}">${email}</a></p>
            <p><strong>Company:</strong> ${company || 'Not specified'}</p>
            <p><strong>Interest Type:</strong> ${interestType}</p>
            <p><strong>Submitted:</strong> ${new Date().toISOString()}</p>
          </div>
          
          <div style="background: #fff; border: 1px solid #dee2e6; padding: 20px; border-radius: 8px;">
            <h3 style="margin-top: 0; color: #495057;">Message</h3>
            <p style="white-space: pre-wrap; margin: 0;">${message}</p>
          </div>
        </div>
      `
    };

    // Email to sender (confirmation)
    const confirmationEmail = {
      from: process.env.EMAIL_FROM,
      to: email,
      subject: 'Thank you for contacting me - Bernard Lusale',
      html: `
        <div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
          <h2 style="color: #667eea; border-bottom: 2px solid #667eea; padding-bottom: 10px;">
            Thank You for Getting in Touch!
          </h2>
          
          <p>Hi ${name},</p>
          
          <p>Thank you for reaching out through my portfolio website. I've received your message regarding <strong>${interestType.toLowerCase()}</strong> and I appreciate your interest.</p>
          
          <p>I'll review your message and get back to you within 24-48 hours.</p>
          
          <div style="margin-top: 30px; padding: 20px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border-radius: 8px; color: white;">
            <p style="margin: 0; font-weight: bold;">Bernard Lusale</p>
            <p style="margin: 5px 0 0 0; opacity: 0.9;">Full Stack Developer</p>
            <p style="margin: 5px 0 0 0; opacity: 0.9;">
              <a href="mailto:bernardlusale20@gmail.com" style="color: white;">bernardlusale20@gmail.com</a>
            </p>
          </div>
        </div>
      `
    };

    // Send emails
    await transporter.sendMail(notificationEmail);
    await transporter.sendMail(confirmationEmail);

    res.status(200).json({ 
      message: 'Message sent successfully!',
      id: Date.now() // Simple ID for tracking
    });

  } catch (error) {
    console.error('Email error:', error);
    res.status(500).json({ 
      message: 'Failed to send message. Please try again.' 
    });
  }
}