
    // ** React Imports
    import { useEffect, useState} from 'react'

    // ** MUI Imports
    import Tab from '@mui/material/Tab'
    import Card from '@mui/material/Card'
    
    import TabList from '@mui/lab/TabList'
    import TabPanel from '@mui/lab/TabPanel'
    
    import TabContext from '@mui/lab/TabContext'
    import MuiMenuItem from '@mui/material/MenuItem'
    import MuiMenu from '@mui/material/Menu'
    
    import { styled } from '@mui/material/styles'
    import useMediaQuery from '@mui/material/useMediaQuery'
    import CardContent from '@mui/material/CardContent'
   
    import Typography from '@mui/material/Typography'
    import Box from '@mui/material/Box'
    import IconButton from '@mui/material/IconButton'
    
    
    // ** Third Party Components
    import PerfectScrollbarComponent from 'react-perfect-scrollbar'
    
    // ** Icon Imports
    import Icon from 'src/@core/components/icon'
    
    import CustomChip from 'src/@core/components/mui/chip'
    import CustomAvatar from 'src/@core/components/mui/avatar'
    
    
    import axios from 'axios'
    import { Popover, TextField } from '@mui/material'

    
    // ** Styled Menu component
    const Menu = styled(MuiMenu)(({ theme }) => ({
      '& .MuiMenu-paper': {
        width: 1200,
        height:400,
        overflow: 'hidden',
        marginTop: theme.spacing(4),
        [theme.breakpoints.down('sm')]: {
          width: '100%'
        }
      },
      '& .MuiMenu-list': {
        padding: 0
      }
    }))
    
    // ** Styled MenuItem component
    const MenuItem = styled(MuiMenuItem)(({ theme }) => ({
      paddingTop: theme.spacing(3),
      paddingBottom: theme.spacing(3),
      // '&:not(:last-of-type)': {
      //   borderBottom: `1px solid ${theme.palette.divider}`
      // }
    }))
    
    // ** Styled PerfectScrollbar component
    const PerfectScrollbar = styled(PerfectScrollbarComponent)({
      MaxWidth: 900
    })
    
    // ** Styled Avatar component
    const Avatar = styled(CustomAvatar)({
      width: 38,
      height: 38,
      fontSize: '1.125rem'
    })
    // ** Styled MenuIcon component
    const MenuIcon = styled(Menu)({
      width: 28,
      height: 28,
      fontSize: '1.125rem'
    })
    
    
    // ** Styled component for the title in MenuItems
    const MenuItemTitle = styled(Typography)(({ theme }) => ({
      fontWeight: 900,
      flex: '1 1 100%',
      overflow: 'hidden',
      fontSize: '0.875rem',
      whiteSpace: 'nowrap',
      textOverflow: 'ellipsis',
      marginBottom: theme.spacing(0.75)
    }))
    
    
    
    // ** Styled component for the subtitle in MenuItems
    const MenuItemSubtitle = styled(Typography)({
      flex: '1 1 100%',
      overflow: 'hidden',
      whiteSpace: 'nowrap',
      textOverflow: 'ellipsis'
    })
    
    const ScrollWrapper = ({ children, hidden }) => {
      if (hidden) {
        return <Box sx={{ maxHeight: 300, overflowY:'auto', overfllowX: 'hidden' }}>{children}</Box>
      } else {
        return <PerfectScrollbar options={{ wheelPropagation: false, suppressScrollX: true }}>{children}</PerfectScrollbar>
      }
    } 
    const options = [
      'Mark as read',
      'Toggle Star',
      'Archive'
      
    ]
    const ITEM_HEIGHT = 10;
    
    
    
    const Message = (props) => {
    
    
      const [value, setValue] = useState('All')
      const [inputSearch,setinputSearch]=useState("")
      const [anchorEl, setAnchorEl] = useState(null)
       const[message,setData]=useState([])
      // const { message} = props
       useEffect(()=>{
        axios
        .get("https://webapidev.aitalkx.com/portaladmin/notification/GetAllNotifications?userId=60da8f8f195197515042a1f2&portalId=c8dce908-74b1-4111-a809-a5e6995db660")
        .then((res)=> setData(res.data))
        },[])
      //Hook
      const hidden = useMediaQuery(theme => theme.breakpoints.down('lg'))
      const handleClick= event => {
        setAnchorEl(event.currentTarget)
      }
    
      const handleClose = () => {
        setAnchorEl(null)
      }
    
      const handleTabsChange = (event, newValue) => {
        setValue(newValue)
      }
    
    
    
      return (
        <>
        <Card sx={{height:500,
        width:{
               xs:600,
               sm:700,
               md:800,
               lg:900,
               xl:1000
        }
        }}>
          <TabContext value={value}>
            <TabList
              variant='scrollable'
              scrollButtons={false}
              onChange={handleTabsChange}
              sx={{ borderBottom: theme => `1px solid ${theme.palette.divider}`  }}
            >
              <Tab value='All' label='All' />
              <Tab value='Starred' label='Starred' />
              <Tab value='Archived' label='Archived' />
            </TabList>
            <Card sc={{width:900}}>
            <CardContent sx={{marginLeft:5}}>
            <Box sx={{display:'flex',flexWrap:'wrap',alignItems:'center'}}>
            <TextField
            size='small'
            value={inputSearch}
            sx={{ mr: 4, mb:1, width:"600px" }}
            placeholder='Search User'
            onChange={(e)=>setinputSearch(e.target.value)}/>
        </Box>
            </CardContent>
            </Card>
              <CardContent>
                <TabPanel value='All'>
               
              </TabPanel>
             
              <Menu>        
            <MenuItem
              disableRipple
              disableTouchRipple
              sx={{
                py: 3.5,
                borderBottom: 0,
                cursor: 'default',
                userSelect: 'auto',
                backgroundColor: 'transparent !important',
                borderTop: theme => `1px solid ${theme.palette.divider}`
              }}
            >
              
            </MenuItem>
            
            </Menu>
           
               
                {/* <Menu
            keepMounted
            id='long-menu'
            anchorEl={anchorEl}
            onClose={handleClose}
            open={Boolean(anchorEl)}
            PaperProps={{
              style: {
                maxHeight: ITEM_HEIGHT * 4.5
              }
            }}
          >
            {options.map(option => (
              <MenuItem key={option} selected={option === 'Pyxis'} onClick={handleClose}>
                {option}
              </MenuItem>
            ))}
          </Menu> */}
              
              
                <TabPanel value='Starred'>
                </TabPanel>
                <TabPanel value='Archived'>
                </TabPanel>
                <ScrollWrapper hidden={!hidden}>
               
                {
              message.filter((value)=>{
                if(inputSearch===""){
                  return value
                   }
                   else if(value.Subject.toLowerCase().includes(inputSearch.toLowerCase())){
                    return value
                   }
              })
            .map((data, index) => (
            <MenuItem key={index}>
              <Box sx={{ width: '100%', display: 'flex', alignItems: 'center' }}>
               {/* <IconButton>{message.icon}</IconButton> */}
               <Icon icon='material-symbols:arrow-right-rounded' />
               <Icon icon='mdi:star-outline' />
               <Avatar alt="Remy Sharp" src='/images/avatars/4.png' />
               
                <Box sx={{ mx: 1, flex: '1', display: 'flex', overflow: 'hidden', flexDirection: 'column' }}>
                
                  <MenuItemTitle>{data.From}</MenuItemTitle>
                  <MenuItemSubtitle variant='body2'>{data.DisplayCreatedDate}</MenuItemSubtitle>
                </Box>
                <Box sx={{mx:'0',flex: '1', display:'flex',overflow:'hidden', flexDirection:'column'}}>
                <MenuItemTitle>{data.Subject}</MenuItemTitle>
            {/* <MenuItemSubtitle variant='body2'>{messages.subtitle1}</MenuItemSubtitle> */}
                
                </Box>
                <Box>
                <IconButton aria-label='more' aria-controls='long-menu' aria-haspopup='true' onClick={handleClick}>
                <Icon icon='mdi:dots-horizontal' />
                 </IconButton>
                 </Box>
      </Box>
              
            </MenuItem>
          ))}
              </ScrollWrapper>
              
             
               
                <Popover
            keepMounted
            id='long-menu'
            anchorEl={anchorEl}
            onClose={handleClose}
            open={Boolean(anchorEl)}
            anchorOrigin={{
          vertical: 'top',
          horizontal: 'center',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'center',
        }}
            PaperProps={{
              style: {
                maxHeight: ITEM_HEIGHT * 15.5,
                
              }
            }}
          >
             <Box>
            {options.map(option => (
              <MenuItem key={option} selected={option === 'Pyxis'} onClick={handleClose}>
                {option}
              </MenuItem>
            ))}
            </Box>
          </Popover>
             
              </CardContent>
          </TabContext>
        </Card>
        
        </>
      )
        }
    
    export default Message
    
          
        
    