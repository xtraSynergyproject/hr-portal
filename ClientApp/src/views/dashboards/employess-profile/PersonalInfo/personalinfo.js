import Personal_edit_modal from './Personal_edit_modal'
// export default PaySlip
import { useState, useEffect } from 'react'
// ** MUI Components
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import { styled } from '@mui/material/styles'
import CardMedia from '@mui/material/CardMedia'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
// ** Third Party Imports
import axios from 'axios'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

const ProfilePicture = styled('img')(({ theme }) => ({
  width: 120,
  height: 120,
  borderRadius: theme.shape.borderRadius,
  border: `5px solid ${theme.palette.common.white}`,
  [theme.breakpoints.down('md')]: {
    marginBottom: theme.spacing(4)
  }
}))

const UserProfileHeader = () => {
  // ** State
  const [data, setData] = useState(null)
  useEffect(() => {
    axios.get('/pages/profile-header').then(response => {
      setData(response.data)
    })
  }, [])


  // ** State
  const [datta, seetData] = useState(null)
  useEffect(() => {
    axios
      .get(
        'https://webapidev.aitalkx.com/chr/hrdirect/EmployeeProfile?userId=60da8f8f195197515042a1f2&portalName=HR&personId=0a11a928-4f66-41b4-aa44-150d1470ef7e'
      )
      .then(response => {
        setData(response.data)
        localStorage.setItem('userProfile', JSON.stringify(response.data))
        console.log(response.data, 'profile data')
      })
  }, [])
  console.log(data, 'data')
  const designationIcon = data?.designationIcon || 'mdi:briefcase-outline'

  return data !== null ? (
    <>
      {/* <ProfilePicture src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQKJQp8ndvEkIa-u1rMgJxVc7BBsR11uSLHGA&usqp=CAU" alt='profile-picture' />  */}

      <Box sx={{ width: '100%', display: 'flex', ml: { xs: 0, md: 6 }, alignItems: 'flex-end', flexWrap: ['wrap', 'nowrap'], justifyContent: ['center', 'space-between'] }}>

        <Box sx={{ mb: [6, 0], display: 'flex', flexDirection: 'column', alignItems: ['center', 'flex-start'] }}>

          <Box
            sx={{
              display: 'flex',
              flexWrap: 'wrap',
              justifyContent: ['center', 'flex-start']
            }}
          >

          </Box>
        </Box>
      </Box>

      <Card sx={{ display: "flex" }} className="user_profile_grid">

        <Box className='user_profile_box box_one' >
          <Typography sx={{ display: 'flex', mx: 2, mb: 2, fontSize: 18 }} >
            <Box sx={{ ml: 3, }}>
              <h4> Basic Info</h4>
            </Box>
          </Typography>
        </Box>

      </Card>
      <Card sx={{py:"10px",display:"flex"}} className="user_profile_grid">
      <Box sx={{ minWidth: '6%', display:"flex", justifyContent:"center" }}>
       
      </Box>
      
      <Box className='user_profile_box box_one' sx={{  minWidth: '30%' ,mb:2}}>
        <Typography sx={{ display: 'flex', mx: 2, mb:2 }}>
          
          <Box sx={{ ml: 3 }}>
          Person No:
          </Box>
         
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
          Personal Email:
          </Box>
      
        </Typography>
      </Box>
      <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.PersonNo} </b>
          </Box>
        </Typography>
        <Typography sx={{ display: 'flex', m: 2 }}>
        <Box sx={{ ml: 3 }}>
          <b>{data.PersonalEmail} </b>
          </Box>
        </Typography>
      </Box>
      <Box className='user_profile_box box_two' sx={{  minWidth: '15%' }}>
        <Typography sx={{ display: 'flex', m: 2 }}>
         <Box sx={{ ml: 3 }}>
       Person Full Name:
          </Box>
        </Typography>
      </Box>
      <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.PersonFullName} </b>
          </Box>
        </Typography>      
      </Box>
       </Card>
      <Card sx={{ display: "flex" }} className="user_profile_grid">
        <Box sx={{ minWidth: '60%', m: 2, }}>
          <Typography sx={{ display: 'flex', mx: 2, mb: 2, fontSize: 18 }}>
            {/* <PersonOutlinedIcon /> */}
            <Box sx={{ ml: 2 }}>
              <h4>Personal Details</h4>
            </Box>
          </Typography>
        </Box>
        <Box sx={{ m: 2, }}>
          <Typography sx={{ display: 'flex', m: 2, }}>

            <Box sx={{ ml: 1 }}>
              <Personal_edit_modal />
            </Box>
          </Typography>
        </Box>
      </Card>
      <Card sx={{ display: "flex" }} className="user_profile_grid">
       

      
      <Box sx={{ minWidth: '6%', display:"flex", justifyContent:"center" }}>
       
      </Box>
      
      <Box className='user_profile_box box_one' sx={{  minWidth: '30%' ,mb:2}}>
        <Typography sx={{ display: 'flex', mx: 2, mb:2 }}>
        
          <Box sx={{ ml: 3 }}>
          Title:
          </Box>
         
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          Gender: 
          </Box>
      
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          Marital Status:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          Date of Birth:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          Contact Country Name:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', mt: 2,mx:2 }}>
          
          <Box sx={{ ml: 3 }}>
            Status:
          </Box>
        </Typography>
      </Box>
      <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.Title} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.Gender} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.MaritalStatus} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.DateOfBirth} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.ContactCountryName}null </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
          <b>{data.Status} </b>
          </Box>
        </Typography>
      </Box>
      <Box className='user_profile_box box_two' sx={{  minWidth: '15%' }}>
        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          Person Full Name:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
          Nationality Name:
 
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
          Religion:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
    
          <Box sx={{ ml: 3 }}>
           Personal Email:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
    
          <Box sx={{ ml: 3 }}>
           Mobile:
          </Box>
        </Typography>

     
      </Box>

      <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.PersonFullName} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
           <b>{data.NationalityName} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.Religion} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
            <b>{data.PersonalEmail} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
         <b>{data. Mobile} </b>
          </Box>
        </Typography>
      </Box>

    
      </Card>




      <Card sx={{ display: "flex" }} className="user_profile_grid">



        <Box className='user_profile_box box_one' >
          <Typography sx={{ display: 'flex', mx: 2, mb: 2, fontSize: 18 }} >
            <Box sx={{ ml: 3, }}>
              <h4>Present Country Address</h4>
            </Box>
          </Typography>
        </Box>

      </Card>
      <Card sx={{ display: "flex" }} className="user_profile_grid">
       

      
       <Box sx={{ minWidth: '6%', display:"flex", justifyContent:"center" }}>
        
       </Box>
       
       <Box className='user_profile_box box_one' sx={{  minWidth: '30%' ,mb:2}}>
         <Typography sx={{ display: 'flex', mx: 2, mb:2 }}>
         
           <Box sx={{ ml: 3 }}>
           Unit Number:

           </Box>
          
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           Street Name:
           </Box>
       
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           Postal Code:
           </Box>
         </Typography>
       </Box>
       <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           <b>{data.HomeUnitNumber} </b>
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           <b>{data.HomeStreetName} </b>
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           <b>{data.PresentPostalCode} </b>
           </Box>
         </Typography>
 
           
         
 
      
 
       
       </Box>
       <Box className='user_profile_box box_two' sx={{  minWidth: '15%' }}>
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
          Building Number:
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
         
           <Box sx={{ ml: 3 }}>
           City:
  
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
         
           <Box sx={{ ml: 3 }}>
           Additional Number:
           </Box>
         </Typography>
 
      
 
      
       </Box>
 
       <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           <b>{data.PresentBuildingNumber} </b>
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
            <b>{data.NationalityName} </b>
           </Box>
         </Typography>
 
      
 
     
 
         <Typography sx={{ display: 'flex', m: 2 }}>
         
           <Box sx={{ ml: 3 }}>
          <b>{data. Mobile} </b>
           </Box>
         </Typography>
       </Box>
 
     
       </Card>

      <Card sx={{ display: "flex" }} className="user_profile_grid">



        <Box className='user_profile_box box_one' >
          <Typography sx={{ display: 'flex', mx: 2, mb: 2, fontSize: 18 }} >
            <Box sx={{ ml: 3, }}>

              <h4>Home Country Address</h4>
            </Box>
          </Typography>
        </Box>
      </Card>
      <Card sx={{ display: "flex" }} className="user_profile_grid">
       

      
       <Box sx={{ minWidth: '6%', display:"flex", justifyContent:"center" }}>
        
       </Box>
       
       <Box className='user_profile_box box_one' sx={{  minWidth: '30%' ,mb:2}}>
         <Typography sx={{ display: 'flex', mx: 2, mb:2 }}>
         
           <Box sx={{ ml: 3 }}>
           Unit Number:
           </Box>
          
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           Street Name: 
           </Box>
       
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           Postal Code:
           </Box>
         </Typography>
       </Box>
       <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           <b>{data.HomeUnitNumber} </b>
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           <b>{data.HomeStreetName} </b>
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           <b>{data.HomePostalCode} </b>
           </Box>
         </Typography>
 
     
 
      </Box>
       <Box className='user_profile_box box_two' sx={{  minWidth: '15%' }}>
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
          Building Number:
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
         
           <Box sx={{ ml: 3 }}>
            City:
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
         
           <Box sx={{ ml: 3 }}>
           Additional Number:
           </Box>
         </Typography>
 
 
      
       </Box>
 
       <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           <b>{data.HomeBuildingNumber} </b>
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
            <b>{data.HomeCity} </b>
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           <b>{data.HomeAdditionalNumber} </b>
           </Box>
         </Typography>
 
      
 
       </Box>
 
     
       </Card>
      <Card sx={{ display: "flex" }} className="user_profile_grid">
        <Box className='user_profile_box box_one' >
          <Typography sx={{ display: 'flex', mx: 2, mb: 2, fontSize: 18 }} >
            <Box sx={{ ml: 3, }}>
              <h4>Emergency Contact Info 1</h4>
            </Box>
          </Typography>
        </Box>
      </Card>
      <Card sx={{ display: "flex" }} className="user_profile_grid">
       

      
       <Box sx={{ minWidth: '6%', display:"flex", justifyContent:"center" }}>
        
       </Box>
       
       <Box className='user_profile_box box_one' sx={{  minWidth: '30%' ,mb:2}}>
         <Typography sx={{ display: 'flex', mx: 2, mb:2 }}>
         
           <Box sx={{ ml: 3 }}>
           Emergency Contact Name 1:
           </Box>
          
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
         Relationship 1:
           </Box>
         </Typography>
       </Box>
       <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           <b>{data.EmergencyContactName1} </b>
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           <b>{data.Relationship1} </b>
           </Box>
         </Typography>
 
      
 
     
       </Box>
       <Box className='user_profile_box box_two' sx={{  minWidth: '15%' }}>
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           Emergency Contact No 1:

           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
         
           <Box sx={{ ml: 3 }}>
           Country Name 1:
           </Box>
         </Typography>
       </Box>
 
       <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           <b>{data.EmergencyContactNo1} </b>
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
            <b>{data.NationalityName} </b>
           </Box>
         </Typography>
 
     
       </Box>
 
     
       </Card>
      <Card sx={{ display: "flex" }} className="user_profile_grid">
        <Box className='user_profile_box box_one' >
          <Typography sx={{ display: 'flex', mx: 2, mb: 2, fontSize: 18 }} >
            <Box sx={{ ml: 3, }}>
              <h4>Emergency Contact Info 2</h4>
            </Box>
          </Typography>
        </Box>
      </Card>
      <Card sx={{ display: "flex" }} className="user_profile_grid">
       

      
       <Box sx={{ minWidth: '6%', display:"flex", justifyContent:"center" }}>
        
       </Box>
       
       <Box className='user_profile_box box_one' sx={{  minWidth: '30%' ,mb:2}}>
         <Typography sx={{ display: 'flex', mx: 2, mb:2 }}>
         
           <Box sx={{ ml: 3 }}>
           Emergency Contact Name 2:
           </Box>
          
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           Relationship 2:

           </Box>
       
         </Typography>
       </Box>
       <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           <b>{data.EmergencyContactName2} </b>
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           <b>{data.Relationship } </b>
           </Box>
         </Typography>

 
       </Box>
       <Box className='user_profile_box box_two' sx={{  minWidth: '15%' }}>
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           Emergency Contact No 2:
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
         
           <Box sx={{ ml: 3 }}>
           Country Name 2:

  
           </Box>
         </Typography>
 
    
 
         
 
      
       </Box>
 
       <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
           <b>{data.EmergencyContactNo1} </b>
           </Box>
         </Typography>
 
         <Typography sx={{ display: 'flex', m: 2 }}>
           
           <Box sx={{ ml: 3 }}>
            <b>{data.EmergencyContactCountryName2} </b>
           </Box>
         </Typography>
 
       </Box>
       </Card>
      <Box sx={{ width: '100%', display: 'flex', ml: { xs: 0, md: 6 }, alignItems: 'flex-end', flexWrap: ['wrap', 'nowrap'], justifyContent: ['center', 'space-between'] }}>

        <Box sx={{ mb: [6, 0], display: 'flex', flexDirection: 'column', alignItems: ['center', 'flex-start'] }}>

          <Box
            sx={{
              display: 'flex',
              flexWrap: 'wrap',
              justifyContent: ['center', 'flex-start']
            }}
          >

          </Box>
        </Box>
      </Box>
    </>


  ) : null
}

export default UserProfileHeader

