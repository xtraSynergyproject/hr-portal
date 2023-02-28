// ** MUI Imports
import Box from '@mui/material/Box'
import IconButton from '@mui/material/IconButton'
// ** Icon Imports
import Icon from 'src/@core/components/icon'

// ** Components

import Message from './Message'


const message=[
  {
    
    avatarAlt: 'Flora',
    title: 'Administrator',
    title1:'To notify',
    subtitle1:'5 hours ago',
    avatarImg: '/images/avatars/4.png',
    subtitle: 'System Administrator',
    icon:'mdi:dots-horizontal',
    iconAlt:'not',
    icon1:'mdi:star-outline'
  },
  {
    meta: '',
    avatarAlt: 'Flora',
    title: 'Administrator',
    title1:'To notify',
    subtitle1:'5 hours ago',
    avatarImg: '/images/avatars/4.png',
    subtitle: 'System Administrator',
    icon:'mdi:dots-horizontal',
    iconAlt:'not'
  },
  {
    meta: '',
    avatarAlt: 'Flora',
    title: 'Administrator',
    meta:'to notify',
    title1:'test',
    subtitle1:'5 hours ago',
    avatarImg: '/images/avatars/4.png',
    subtitle: 'System Administrator', 
    icon:'mdi:dots-horizontal',
    iconAlt:'not'
  },
  {
    meta: '',
    avatarAlt: 'Flora',
    title: 'Administrator',
    meta:'to notify',
    title1:'Rental Property expiring in 30 days',
    subtitle1:'10 hours ago',
    avatarImg: '/images/avatars/4.png',
    subtitle: 'System Administrator',
    icon:'mdi:dots-horizontal',
    iconAlt:'not'
  },
  {
    meta: '',
    avatarAlt: 'Flora',
    title: 'Administrator',
    meta:'to notify',
    title1:'Rental Property expiring in 60 days',
    subtitle1:'10 hours ago',
    avatarImg: '/images/avatars/4.png',
    subtitle: 'System Administrator',
    icon:'mdi:dots-horizontal',
    iconAlt:'not'
  },
  {
    avatarAlt: 'Flora',
    title: 'Administrator',
    title1:'Service request completed',
    subtitle1:'10 hours ago',
    meta:'to notify',
    avatarImg: '/images/avatars/4.png',
    subtitle: 'System Administrator',
    icon:'mdi:dots-horizontal',
    iconAlt:'not'
  },
  {
  
  avatarAlt: 'Flora',
  title: 'Administrator',
  meta:'to notify',
  title1:'Service request submitted',
  subtitle1:'5 hours ago',
  avatarImg: '/images/avatars/4.png',
  subtitle: 'System Administrator',
  icon:'mdi:dots-horizontal',
  iconAlt:'not'
},
{
  meta: '',
  avatarAlt: 'Flora',
  title: 'Administrator',
  meta:'to notify',
  title1:'Service request submitted',
  subtitle1:'5 hours ago',
  avatarImg: '/images/avatars/4.png',
  subtitle: 'System Administrator', 
  icon:'mdi:dots-horizontal',
  iconAlt:'not'
},
]

const MessageContent=(props) =>{

  console.log("propsMessageContent",props);
  return (
    <Box sx={{ width: '100%', display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
      
      <Box className='actions-right' sx={{ display: 'flex', alignItems: 'center' }} >
        
       
      
      
        <Message  message={message}/>
       
      </Box>
    </Box>
  )
}

export default MessageContent
